using Microsoft.EntityFrameworkCore;
using EmployeeAttendance.Data;
using EmployeeAttendance.Models;

namespace EmployeeAttendance.Services
{
	public class SalaryStructureService : ISalaryStructureService
	{
		private readonly ApplicationDbContext _context;
		public SalaryStructureService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<SalaryStructure>> GetAllAsync()
		{
			return await _context.Set<SalaryStructure>()
				.Include(s => s.Employee)
				.OrderByDescending(s => s.EffectiveFrom)
				.ToListAsync();
		}

		public async Task<SalaryStructure?> GetByIdAsync(int id)
		{
			return await _context.Set<SalaryStructure>()
				.Include(s => s.Employee)
				.FirstOrDefaultAsync(s => s.Id == id);
		}

		public async Task<IEnumerable<SalaryStructure>> GetByEmployeeAsync(int employeeId)
		{
			return await _context.Set<SalaryStructure>()
				.Include(s => s.Employee)
				.Where(s => s.EmployeeId == employeeId)
				.OrderByDescending(s => s.EffectiveFrom)
				.ToListAsync();
		}

		public async Task<SalaryStructure?> GetEffectiveForAsync(int employeeId, DateTime asOfDate)
		{
			return await _context.Set<SalaryStructure>()
				.Where(s => s.EmployeeId == employeeId && s.EffectiveFrom <= asOfDate)
				.OrderByDescending(s => s.EffectiveFrom)
				.FirstOrDefaultAsync();
		}

		public async Task<SalaryStructure> CreateAsync(SalaryStructure structure)
		{
			_context.Set<SalaryStructure>().Add(structure);
			await _context.SaveChangesAsync();
			return structure;
		}

		public async Task<SalaryStructure> UpdateAsync(SalaryStructure structure)
		{
			_context.Set<SalaryStructure>().Update(structure);
			await _context.SaveChangesAsync();
			return structure;
		}

		public async Task DeleteAsync(int id)
		{
			var entity = await _context.Set<SalaryStructure>().FindAsync(id);
			if (entity != null)
			{
				_context.Set<SalaryStructure>().Remove(entity);
				await _context.SaveChangesAsync();
			}
		}
	}
}