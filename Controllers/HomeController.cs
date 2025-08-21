using Microsoft.AspNetCore.Mvc;
using EmployeeAttendance.Services;
using EmployeeAttendance.Models;

namespace EmployeeAttendance.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttendanceService _attendanceService;

        public HomeController(IEmployeeService employeeService, IAttendanceService attendanceService)
        {
            _employeeService = employeeService;
            _attendanceService = attendanceService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetActiveEmployeesAsync();
            var todayAttendance = await _attendanceService.GetAllAttendanceAsync();
            
            var today = DateTime.Today;
            var todayRecords = todayAttendance
                .Where(a => a.Date.Date == today)
                .ToList();
            
            // Business rules:
            // - Present includes: "Present", "Late", "Half-day"
            // - Late is specifically status == "Late"
            // - Absent = TotalActiveEmployees - Present
            var presentStatuses = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Present",
                "Late",
                "Half-day"
            };
            
            ViewBag.TotalEmployees = employees.Count();
            var presentCount = todayRecords.Count(a => presentStatuses.Contains(a.Status ?? string.Empty));
            ViewBag.PresentToday = presentCount;
            ViewBag.AbsentToday = employees.Count() - presentCount;
            ViewBag.LateToday = todayRecords.Count(a => string.Equals(a.Status, "Late", StringComparison.OrdinalIgnoreCase));
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}