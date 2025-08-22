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
			// Get active employees
			var activeEmployees = await _context.Employees
				.Where(e => e.IsActive)
				.Select(e => new { e.Id })
				.ToListAsync();

			int createdCount = 0;
			foreach (var emp in activeEmployees)
			{
				bool exists = await IsDuplicateAsync(emp.Id, year, month);
				if (exists)
				{
					continue;
				}

				var payroll = new Payroll
				{
					EmployeeId = emp.Id,
					Year = year,
					Month = month,
					BasicSalary = 0m,
					Allowances = 0m,
					Deductions = 0m,
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