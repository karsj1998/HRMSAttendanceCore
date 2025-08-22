using Microsoft.AspNetCore.Mvc;
using EmployeeAttendance.Services;
using EmployeeAttendance.Models;

namespace EmployeeAttendance.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,EmployeeId,Department,Position,HireDate,IsActive")] Employee employee)
        {
            if (await _employeeService.EmployeeIdExistsAsync(employee.EmployeeId))
            {
                ModelState.AddModelError(nameof(Employee.EmployeeId), "Employee ID already exists.");
            }

            if (ModelState.IsValid)
            {
                await _employeeService.CreateEmployeeAsync(employee);
                TempData["Toast"] = "Employee saved successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,EmployeeId,Department,Position,HireDate,IsActive")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (await _employeeService.EmployeeIdExistsAsync(employee.EmployeeId, excludeEmployeeId: employee.Id))
            {
                ModelState.AddModelError(nameof(Employee.EmployeeId), "Employee ID already exists.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.UpdateEmployeeAsync(employee);
                    TempData["Toast"] = "Employee updated successfully.";
                }
                catch
                {
                    if (await _employeeService.GetEmployeeByIdAsync(id) == null)
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
            return View(employee);
        }

        // Remote validation endpoint for EmployeeId uniqueness
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmployeeId(string employeeId, int? id)
        {
            var exists = await _employeeService.EmployeeIdExistsAsync(employeeId, excludeEmployeeId: id);
            return exists ? Json($"Employee ID '{employeeId}' is already taken.") : Json(true);
        }

        // POST: Employees/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            TempData["Toast"] = "Employee deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}