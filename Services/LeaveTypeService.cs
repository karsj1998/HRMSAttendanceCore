using Microsoft.EntityFrameworkCore;
using EmployeeAttendance.Data;
using EmployeeAttendance.Models;

namespace EmployeeAttendance.Services
{
	public class LeaveTypeService : ILeaveTypeService
	{
		private readonly ApplicationDbContext _context;
		public LeaveTypeService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<LeaveType>> GetAllAsync()
		{
			return await _context.LeaveTypes.OrderBy(l => l.Name).ToListAsync();
		}

		public async Task<LeaveType?> GetByIdAsync(int id)
		{
			return await _context.LeaveTypes.FindAsync(id);
		}

		public async Task<LeaveType> CreateAsync(LeaveType type)
		{
			_context.LeaveTypes.Add(type);
			await _context.SaveChangesAsync();
			return type;
		}

		public async Task<LeaveType> UpdateAsync(LeaveType type)
		{
			_context.LeaveTypes.Update(type);
			await _context.SaveChangesAsync();
			return type;
		}

		public async Task DeleteAsync(int id)
		{
			var entity = await _context.LeaveTypes.FindAsync(id);
			if (entity != null)
			{
				_context.LeaveTypes.Remove(entity);
				await _context.SaveChangesAsync();
			}
		}
	}
}