using Microsoft.EntityFrameworkCore;
using EmployeeAttendance.Data;
using EmployeeAttendance.Models;

namespace EmployeeAttendance.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attendance>> GetAllAttendanceAsync()
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByEmployeeAsync(int employeeId)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetWeeklyAttendanceAsync(int employeeId, DateTime weekStart)
        {
            var weekEnd = weekStart.AddDays(6);
            return await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && a.Date >= weekStart && a.Date <= weekEnd)
                .OrderBy(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetMonthlyAttendanceAsync(int employeeId, int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            
            return await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && a.Date >= startDate && a.Date <= endDate)
                .OrderBy(a => a.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetAttendanceByDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.EmployeeId == employeeId && a.Date >= startDate && a.Date <= endDate)
                .OrderBy(a => a.Date)
                .ToListAsync();
        }

        public async Task<Attendance> CreateAttendanceAsync(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<Attendance> UpdateAttendanceAsync(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task DeleteAttendanceAsync(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Attendance?> GetAttendanceByEmployeeAndDateAsync(int employeeId, DateTime date)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date.Date == date.Date);
        }
    }
}