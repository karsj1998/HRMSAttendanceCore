using EmployeeAttendance.Models;

namespace EmployeeAttendance.Services
{
	public interface ISalaryStructureService
	{
		Task<IEnumerable<SalaryStructure>> GetAllAsync();
		Task<SalaryStructure?> GetByIdAsync(int id);
		Task<IEnumerable<SalaryStructure>> GetByEmployeeAsync(int employeeId);
		Task<SalaryStructure?> GetEffectiveForAsync(int employeeId, DateTime asOfDate);
		Task<SalaryStructure> CreateAsync(SalaryStructure structure);
		Task<SalaryStructure> UpdateAsync(SalaryStructure structure);
		Task DeleteAsync(int id);
	}
}