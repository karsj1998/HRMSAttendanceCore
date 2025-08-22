using Microsoft.AspNetCore.Mvc;
using EmployeeAttendance.Models;
using EmployeeAttendance.Services;

namespace EmployeeAttendance.Controllers
{
	public class PayrollController : Controller
	{
		private readonly IPayrollService _payrollService;
		private readonly IEmployeeService _employeeService;

		public PayrollController(IPayrollService payrollService, IEmployeeService employeeService)
		{
			_payrollService = payrollService;
			_employeeService = employeeService;
		}

		public async Task<IActionResult> Index(int? employeeId)
		{
			IEnumerable<Payroll> payrolls;
			if (employeeId.HasValue)
			{
				payrolls = await _payrollService.GetByEmployeeAsync(employeeId.Value);
			}
			else
			{
				payrolls = await _payrollService.GetAllAsync();
			}
			return View(payrolls);
		}

		public async Task<IActionResult> Create()
		{
			ViewBag.Employees = await _employeeService.GetActiveEmployeesAsync();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("EmployeeId,Year,Month,BasicSalary,Allowances,Deductions,PaymentDate,Status")] Payroll payroll)
		{
			ModelState.Remove("Employee");
			if (await _payrollService.IsDuplicateAsync(payroll.EmployeeId, payroll.Year, payroll.Month))
			{
				ModelState.AddModelError(string.Empty, "A payroll record already exists for this employee and period.");
			}
			if (ModelState.IsValid)
			{
				try
				{
					await _payrollService.CreateAsync(payroll);
					return RedirectToAction(nameof(Index));
				}
				catch (InvalidOperationException ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			ViewBag.Employees = await _employeeService.GetActiveEmployeesAsync();
			return View(payroll);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var payroll = await _payrollService.GetByIdAsync(id);
			if (payroll == null)
			{
				return NotFound();
			}
			ViewBag.Employees = await _employeeService.GetActiveEmployeesAsync();
			return View(payroll);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,Year,Month,BasicSalary,Allowances,Deductions,PaymentDate,Status")] Payroll payroll)
		{
			if (id != payroll.Id)
			{
				return NotFound();
			}
			if (await _payrollService.IsDuplicateAsync(payroll.EmployeeId, payroll.Year, payroll.Month, payroll.Id))
			{
				ModelState.AddModelError(string.Empty, "A payroll record already exists for this employee and period.");
			}
            ModelState.Remove("Employee");
            if (ModelState.IsValid)
			{
				try
				{
					await _payrollService.UpdateAsync(payroll);
					return RedirectToAction(nameof(Index));
				}
				catch (InvalidOperationException ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			ViewBag.Employees = await _employeeService.GetActiveEmployeesAsync();
			return View(payroll);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var payroll = await _payrollService.GetByIdAsync(id);
			if (payroll == null)
			{
				return NotFound();
			}
			return View(payroll);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _payrollService.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}

		// Generate monthly payroll for all employees
		[HttpGet]
		public IActionResult Generate()
		{
			var now = DateTime.Now;
			ViewBag.DefaultYear = now.Year;
			ViewBag.DefaultMonth = now.Month;
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Generate(int year, int month)
		{
			if (year < 2000 || year > 2100 || month < 1 || month > 12)
			{
				ModelState.AddModelError(string.Empty, "Invalid period selected.");
			}
			if (!ModelState.IsValid)
			{
				ViewBag.DefaultYear = year;
				ViewBag.DefaultMonth = month;
				return View();
			}
			var created = await _payrollService.GenerateMonthlyAsync(year, month);
			TempData["Message"] = $"Generated {created} payroll records for {new DateTime(year, month, 1):MMMM yyyy}.";
			return RedirectToAction(nameof(Index));
		}
	}
}