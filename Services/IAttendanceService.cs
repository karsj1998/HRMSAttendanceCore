using EmployeeAttendance.Models;

namespace EmployeeAttendance.Services
{
    public interface IAttendanceService
    {
        Task<IEnumerable<Attendance>> GetAllAttendanceAsync();
        Task<IEnumerable<Attendance>> GetAttendanceByEmployeeAsync(int employeeId);
        Task<IEnumerable<Attendance>> GetWeeklyAttendanceAsync(int employeeId, DateTime weekStart);
        Task<IEnumerable<Attendance>> GetMonthlyAttendanceAsync(int employeeId, int year, int month);
        Task<IEnumerable<Attendance>> GetAttendanceByDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<Attendance> CreateAttendanceAsync(Attendance attendance);
        Task<Attendance> UpdateAttendanceAsync(Attendance attendance);
        Task DeleteAttendanceAsync(int id);
        Task<Attendance?> GetAttendanceByEmployeeAndDateAsync(int employeeId, DateTime date);
        Task<bool> IsDuplicateAttendanceAsync(int employeeId, DateTime date, int? excludeId = null);
    }
}