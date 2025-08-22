using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendance.Models
{
	public class LeaveRequest
	{
		public int Id { get; set; }
		[Required]
		public int EmployeeId { get; set; }
		[Required]
		public int LeaveTypeId { get; set; }
		[DataType(DataType.Date)]
		public DateTime StartDate { get; set; }
		[DataType(DataType.Date)]
		public DateTime EndDate { get; set; }
		[Range(0, 365)]
		public decimal Days { get; set; }
		[StringLength(20)]
		public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
		[StringLength(500)]
		public string? Reason { get; set; }
		[StringLength(100)]
		public string? ApprovedBy { get; set; }
		public DateTime? ApprovedAt { get; set; }

		public virtual Employee Employee { get; set; } = null!;
		public virtual LeaveType LeaveType { get; set; } = null!;
	}
}