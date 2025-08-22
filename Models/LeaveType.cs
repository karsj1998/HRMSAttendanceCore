using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendance.Models
{
	public class LeaveType
	{
		public int Id { get; set; }

		[Required]
		[StringLength(50)]
		public string Name { get; set; } = string.Empty;

		[Range(0, 365)]
		public decimal AnnualAllocation { get; set; }
	}
}