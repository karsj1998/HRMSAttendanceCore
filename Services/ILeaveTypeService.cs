using EmployeeAttendance.Models;

namespace EmployeeAttendance.Services
{
	public interface ILeaveTypeService
	{
		Task<IEnumerable<LeaveType>> GetAllAsync();
		Task<LeaveType?> GetByIdAsync(int id);
		Task<LeaveType> CreateAsync(LeaveType type);
		Task<LeaveType> UpdateAsync(LeaveType type);
		Task DeleteAsync(int id);
	}
}