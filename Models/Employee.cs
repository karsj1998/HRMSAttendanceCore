using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendance.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Position { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

        // Computed property for full name
        public string FullName => $"{FirstName} {LastName}";
    }
}