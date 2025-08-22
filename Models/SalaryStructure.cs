using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendance.Models
{
	public class SalaryStructure
	{
		public int Id { get; set; }

		[Required]
		public int EmployeeId { get; set; }

		[Range(0, double.MaxValue)]
		[DataType(DataType.Currency)]
		public decimal Basic { get; set; }

		[Range(0, double.MaxValue)]
		[DataType(DataType.Currency)]
		public decimal Hra { get; set; }

		[Range(0, double.MaxValue)]
		[DataType(DataType.Currency)]
		public decimal DearnessAllowance { get; set; }

		[Range(0, double.MaxValue)]
		[DataType(DataType.Currency)]
		public decimal OtherAllowances { get; set; }

		[Range(0, double.MaxValue)]
		[DataType(DataType.Currency)]
		public decimal Deductions { get; set; }

		[DataType(DataType.Date)]
		public DateTime EffectiveFrom { get; set; } = DateTime.Today;

		// Navigation
		public virtual Employee Employee { get; set; } = null!;

		// Computed aggregates
		public decimal TotalAllowances => Hra + DearnessAllowance + OtherAllowances;
	}
}