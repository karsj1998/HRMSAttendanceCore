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
            var todayRecords = todayAttendance.Where(a => a.Date.Date == today).ToList();
            
            ViewBag.TotalEmployees = employees.Count();
            ViewBag.PresentToday = todayRecords.Count(a => a.Status == "Present");
            ViewBag.AbsentToday = employees.Count() - todayRecords.Count;
            ViewBag.LateToday = todayRecords.Count(a => a.Status == "Late");
            
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