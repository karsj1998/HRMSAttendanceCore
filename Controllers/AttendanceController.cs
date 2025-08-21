using Microsoft.AspNetCore.Mvc;
using EmployeeAttendance.Services;
using EmployeeAttendance.Models;

namespace EmployeeAttendance.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;

        public AttendanceController(IAttendanceService attendanceService, IEmployeeService employeeService)
        {
            _attendanceService = attendanceService;
            _employeeService = employeeService;
        }

        // GET: Attendance
        public async Task<IActionResult> Index()
        {
            var attendance = await _attendanceService.GetAllAttendanceAsync();
            return View(attendance);
        }

        // GET: Attendance/Weekly
        public async Task<IActionResult> Weekly(int? employeeId, DateTime? weekStart)
        {
            var employees = await _employeeService.GetActiveEmployeesAsync();
            ViewBag.Employees = employees;

            if (!employeeId.HasValue)
            {
                employeeId = employees.FirstOrDefault()?.Id;
            }

            if (!weekStart.HasValue)
            {
                weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            }

            if (employeeId.HasValue)
            {
                var weeklyAttendance = await _attendanceService.GetWeeklyAttendanceAsync(employeeId.Value, weekStart.Value);
                ViewBag.SelectedEmployeeId = employeeId.Value;
                ViewBag.WeekStart = weekStart.Value;
                ViewBag.WeekEnd = weekStart.Value.AddDays(6);
                return View(weeklyAttendance);
            }

            return View(new List<Attendance>());
        }

        // GET: Attendance/Monthly
        public async Task<IActionResult> Monthly(int? employeeId, int? year, int? month)
        {
            var employees = await _employeeService.GetActiveEmployeesAsync();
            ViewBag.Employees = employees;

            if (!employeeId.HasValue)
            {
                employeeId = employees.FirstOrDefault()?.Id;
            }

            if (!year.HasValue)
            {
                year = DateTime.Today.Year;
            }

            if (!month.HasValue)
            {
                month = DateTime.Today.Month;
            }

            if (employeeId.HasValue)
            {
                var monthlyAttendance = await _attendanceService.GetMonthlyAttendanceAsync(employeeId.Value, year.Value, month.Value);
                ViewBag.SelectedEmployeeId = employeeId.Value;
                ViewBag.Year = year.Value;
                ViewBag.Month = month.Value;
                ViewBag.MonthName = new DateTime(year.Value, month.Value, 1).ToString("MMMM yyyy");
                return View(monthlyAttendance);
            }

            return View(new List<Attendance>());
        }

        // GET: Attendance/Create
        public async Task<IActionResult> Create()
        {
            var employees = await _employeeService.GetActiveEmployeesAsync();
            ViewBag.Employees = employees;
            return View();
        }

        // POST: Attendance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Date,CheckInTime,CheckOutTime,Status,Notes")] Attendance attendance)
        {
            ModelState.Remove("Employee");
            if (ModelState.IsValid)
            {
                await _attendanceService.CreateAttendanceAsync(attendance);
                return RedirectToAction(nameof(Index));
            }

            var employees = await _employeeService.GetActiveEmployeesAsync();
            ViewBag.Employees = employees;
            return View(attendance);
        }

        // GET: Attendance/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var attendance = await _attendanceService.GetAllAttendanceAsync();
            var attendanceRecord = attendance.FirstOrDefault(a => a.Id == id);
            
            if (attendanceRecord == null)
            {
                return NotFound();
            }

            var employees = await _employeeService.GetActiveEmployeesAsync();
            ViewBag.Employees = employees;
            return View(attendanceRecord);
        }

        // POST: Attendance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,Date,CheckInTime,CheckOutTime,Status,Notes")] Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _attendanceService.UpdateAttendanceAsync(attendance);
                }
                catch
                {
                    var allAttendance = await _attendanceService.GetAllAttendanceAsync();
                    if (!allAttendance.Any(a => a.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var employees = await _employeeService.GetActiveEmployeesAsync();
            ViewBag.Employees = employees;
            return View(attendance);
        }

        // GET: Attendance/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var attendance = await _attendanceService.GetAllAttendanceAsync();
            var attendanceRecord = attendance.FirstOrDefault(a => a.Id == id);
            
            if (attendanceRecord == null)
            {
                return NotFound();
            }

            return View(attendanceRecord);
        }

        // POST: Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _attendanceService.DeleteAttendanceAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}