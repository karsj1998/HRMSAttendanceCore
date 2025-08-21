using Microsoft.EntityFrameworkCore;
using EmployeeAttendance.Data;
using EmployeeAttendance.Models;

namespace EmployeeAttendance.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Attendances)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Attendances)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .OrderBy(e => e.FirstName)
                .ToListAsync();
        }
    }
}