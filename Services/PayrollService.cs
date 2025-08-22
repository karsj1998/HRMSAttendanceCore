using Microsoft.EntityFrameworkCore;
using EmployeeAttendance.Data;
using EmployeeAttendance.Models;

namespace EmployeeAttendance.Services
{
	public class PayrollService : IPayrollService
	{
		private readonly ApplicationDbContext _context;

		public PayrollService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Payroll>> GetAllAsync()
		{
			return await _context.Set<Payroll>()
				.Include(p => p.Employee)
				.OrderByDescending(p => p.Year).ThenByDescending(p => p.Month)
				.ToListAsync();
		}

		public async Task<IEnumerable<Payroll>> GetByEmployeeAsync(int employeeId)
		{
			return await _context.Set<Payroll>()
				.Include(p => p.Employee)
				.Where(p => p.EmployeeId == employeeId)
				.OrderByDescending(p => p.Year).ThenByDescending(p => p.Month)
				.ToListAsync();
		}

		public async Task<Payroll?> GetByIdAsync(int id)
		{
			return await _context.Set<Payroll>()
				.Include(p => p.Employee)
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<Payroll?> GetByEmployeeAndPeriodAsync(int employeeId, int year, int month)
		{
			return await _context.Set<Payroll>()
				.Include(p => p.Employee)
				.FirstOrDefaultAsync(p => p.EmployeeId == employeeId && p.Year == year && p.Month == month);
		}

		public async Task<Payroll> CreateAsync(Payroll payroll)
		{
			if (await IsDuplicateAsync(payroll.EmployeeId, payroll.Year, payroll.Month))
			{
				throw new InvalidOperationException("Duplicate payroll for this employee and period.");
			}
			_context.Set<Payroll>().Add(payroll);
			await _context.SaveChangesAsync();
			return payroll;
		}

		public async Task<Payroll> UpdateAsync(Payroll payroll)
		{
			if (await IsDuplicateAsync(payroll.EmployeeId, payroll.Year, payroll.Month, payroll.Id))
			{
				throw new InvalidOperationException("Duplicate payroll for this employee and period.");
			}
			_context.Set<Payroll>().Update(payroll);
			await _context.SaveChangesAsync();
			return payroll;
		}

		public async Task DeleteAsync(int id)
		{
			var payroll = await _context.Set<Payroll>().FindAsync(id);
			if (payroll != null)
			{
				_context.Set<Payroll>().Remove(payroll);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<bool> IsDuplicateAsync(int employeeId, int year, int month, int? excludeId = null)
		{
			var query = _context.Set<Payroll>().AsQueryable();
			query = query.Where(p => p.EmployeeId == employeeId && p.Year == year && p.Month == month);
			if (excludeId.HasValue)
			{
				query = query.Where(p => p.Id != excludeId.Value);
			}
			return await query.AnyAsync();
		}

		public async Task<int> GenerateMonthlyAsync(int year, int month)
		{
			var periodStart = new DateTime(year, month, 1);
			var periodEnd = periodStart.AddMonths(1).AddDays(-1);
			var activeEmployees = await _context.Employees
				.Where(e => e.IsActive)
				.Select(e => new { e.Id })
				.ToListAsync();

			// Preload latest effective salary structures up to the period for all active employees
			var structures = await _context.SalaryStructures
				.Where(s => s.EffectiveFrom <= periodEnd)
				.GroupBy(s => s.EmployeeId)
				.Select(g => g.OrderByDescending(s => s.EffectiveFrom).First())
				.ToDictionaryAsync(s => s.EmployeeId, s => s);

			// Preload attendance within period for all active employees
			var attendanceByEmployee = await _context.Attendances
				.Where(a => a.Date >= periodStart && a.Date <= periodEnd)
				.GroupBy(a => a.EmployeeId)
				.ToDictionaryAsync(g => g.Key, g => g.ToList());

			// Compute working weekdays in period
			int workingDays = 0;
			for (var d = periodStart; d <= periodEnd; d = d.AddDays(1))
			{
				if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
				{
					workingDays++;
				}
			}
			if (workingDays == 0)
			{
				workingDays = 1; // guard against divide-by-zero for edge months
			}

			decimal MapStatusToUnit(string status)
			{
				if (string.IsNullOrWhiteSpace(status)) return 0m;
				switch (status.Trim().ToLowerInvariant())
				{
					case "present":
					case "late":
						return 1m;
					case "half-day":
					case "halfday":
						return 0.5m;
					case "absent":
					case "leave":
						return 0m;
					default:
						return 0m;
				}
			}

			int createdCount = 0;
			foreach (var emp in activeEmployees)
			{
				bool exists = await IsDuplicateAsync(emp.Id, year, month);
				if (exists)
				{
					continue;
				}

				structures.TryGetValue(emp.Id, out var structure);
				attendanceByEmployee.TryGetValue(emp.Id, out var records);

				decimal attendanceUnits = 0m;
				if (records != null)
				{
					foreach (var rec in records)
					{
						attendanceUnits += MapStatusToUnit(rec.Status);
					}
				}

				var ratio = Math.Min(attendanceUnits / workingDays, 1m);

				var payroll = new Payroll
				{
					EmployeeId = emp.Id,
					Year = year,
					Month = month,
					BasicSalary = (structure?.Basic ?? 0m) * ratio,
					Allowances = ((structure?.Hra ?? 0m) + (structure?.DearnessAllowance ?? 0m) + (structure?.OtherAllowances ?? 0m)) * ratio,
					Deductions = (structure?.Deductions ?? 0m) * ratio,
					Status = "Pending",
					PaymentDate = null
				};

				_context.Set<Payroll>().Add(payroll);
				createdCount++;
			}

			if (createdCount > 0)
			{
				await _context.SaveChangesAsync();
			}

			return createdCount;
		}
	}
}