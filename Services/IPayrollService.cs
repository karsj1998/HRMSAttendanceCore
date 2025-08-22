using EmployeeAttendance.Models;

namespace EmployeeAttendance.Services
{
	public interface IPayrollService
	{
		Task<IEnumerable<Payroll>> GetAllAsync();
		Task<IEnumerable<Payroll>> GetByEmployeeAsync(int employeeId);
		Task<Payroll?> GetByIdAsync(int id);
		Task<Payroll?> GetByEmployeeAndPeriodAsync(int employeeId, int year, int month);
		Task<Payroll> CreateAsync(Payroll payroll);
		Task<Payroll> UpdateAsync(Payroll payroll);
		Task DeleteAsync(int id);
		Task<bool> IsDuplicateAsync(int employeeId, int year, int month, int? excludeId = null);
	}
}