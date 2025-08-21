using EmployeeAttendance.Models;

namespace EmployeeAttendance.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
    }
}