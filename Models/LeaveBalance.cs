using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendance.Models
{
	public class LeaveBalance
	{
		public int Id { get; set; }
		[Required]
		public int EmployeeId { get; set; }
		[Required]
		public int LeaveTypeId { get; set; }
		[Range(2000, 2100)]
		public int Year { get; set; }
		[Range(0, 365)]
		public decimal Remaining { get; set; }

		public virtual Employee Employee { get; set; } = null!;
		public virtual LeaveType LeaveType { get; set; } = null!;
	}
}