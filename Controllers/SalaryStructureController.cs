using Microsoft.AspNetCore.Mvc;
using EmployeeAttendance.Models;
using EmployeeAttendance.Services;

namespace EmployeeAttendance.Controllers
{
	public class SalaryStructureController : Controller
	{
		private readonly ISalaryStructureService _service;
		private readonly IEmployeeService _employeeService;

		public SalaryStructureController(ISalaryStructureService service, IEmployeeService employeeService)
		{
			_service = service;
			_employeeService = employeeService;
		}

		public async Task<IActionResult> Index(int? employeeId)
		{
			IEnumerable<SalaryStructure> items;
			if (employeeId.HasValue)
				items = await _service.GetByEmployeeAsync(employeeId.Value);
			else
				items = await _service.GetAllAsync();
			return View(items);
		}

		public async Task<IActionResult> Create()
		{
			ViewBag.Employees = await _employeeService.GetActiveEmployeesAsync();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("EmployeeId,Basic,Hra,DearnessAllowance,OtherAllowances,Deductions,EffectiveFrom")] SalaryStructure structure)
		{
			ModelState.Remove("Employee");
			if (ModelState.IsValid)
			{
				await _service.CreateAsync(structure);
				return RedirectToAction(nameof(Index));
			}
			ViewBag.Employees = await _employeeService.GetActiveEmployeesAsync();
			return View(structure);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var structure = await _service.GetByIdAsync(id);
			if (structure == null)
				return NotFound();
			ViewBag.Employees = await _employeeService.GetActiveEmployeesAsync();
			return View(structure);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,Basic,Hra,DearnessAllowance,OtherAllowances,Deductions,EffectiveFrom")] SalaryStructure structure)
		{
			if (id != structure.Id)
				return NotFound();
			if (ModelState.IsValid)
			{
				await _service.UpdateAsync(structure);
				return RedirectToAction(nameof(Index));
			}
			ViewBag.Employees = await _employeeService.GetActiveEmployeesAsync();
			return View(structure);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var structure = await _service.GetByIdAsync(id);
			if (structure == null)
				return NotFound();
			return View(structure);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _service.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}