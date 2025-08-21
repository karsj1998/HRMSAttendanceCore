using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendance.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime? CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Present"; // Present, Absent, Late, Half-day

        [StringLength(500)]
        public string? Notes { get; set; }

        // Navigation property
        public virtual Employee Employee { get; set; } = null!;

        // Computed properties
        public TimeSpan? WorkHours
        {
            get
            {
                if (CheckInTime.HasValue && CheckOutTime.HasValue)
                {
                    return CheckOutTime.Value - CheckInTime.Value;
                }
                return null;
            }
        }

        public bool IsLate
        {
            get
            {
                if (CheckInTime.HasValue)
                {
                    var expectedCheckIn = Date.Date.AddHours(9); // Assuming 9 AM is expected check-in
                    return CheckInTime.Value > expectedCheckIn;
                }
                return false;
            }
        }
    }
}