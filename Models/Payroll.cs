using System.ComponentModel.DataAnnotations;

namespace EmployeeAttendance.Models
{
    public class Payroll
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Range(2000, 2100)]
        public int Year { get; set; }

        [Range(1, 12)]
        public int Month { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal BasicSalary { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Allowances { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Deductions { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaymentDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Paid, On Hold

        // Navigation
        public virtual Employee Employee { get; set; } = null!;

        // Computed
        public decimal GrossPay => BasicSalary + Allowances;
        public decimal NetPay => GrossPay - Deductions;

        public string PeriodLabel => new DateTime(Year, Month, 1).ToString("MMMM yyyy");
    }
}