using Microsoft.EntityFrameworkCore;
using EmployeeAttendance.Models;

namespace EmployeeAttendance.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EmployeeId).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.EmployeeId).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Attendance entity
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date).IsRequired();
                entity.HasIndex(e => new { e.EmployeeId, e.Date }).IsUnique();
                
                // Configure relationship
                entity.HasOne(e => e.Employee)
                      .WithMany(e => e.Attendances)
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Payroll entity
            modelBuilder.Entity<Payroll>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => new { p.EmployeeId, p.Year, p.Month }).IsUnique();
                entity.Property(p => p.BasicSalary).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Allowances).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Deductions).HasColumnType("decimal(18,2)");

                entity.HasOne(p => p.Employee)
                      .WithMany()
                      .HasForeignKey(p => p.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@company.com",
                    EmployeeId = "EMP001",
                    Department = "IT",
                    Position = "Software Developer",
                    HireDate = new DateTime(2023, 1, 15),
                    IsActive = true
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@company.com",
                    EmployeeId = "EMP002",
                    Department = "HR",
                    Position = "HR Manager",
                    HireDate = new DateTime(2022, 8, 20),
                    IsActive = true
                },
                new Employee
                {
                    Id = 3,
                    FirstName = "Mike",
                    LastName = "Johnson",
                    Email = "mike.johnson@company.com",
                    EmployeeId = "EMP003",
                    Department = "Finance",
                    Position = "Accountant",
                    HireDate = new DateTime(2023, 3, 10),
                    IsActive = true
                }
            );

            // Seed some attendance data
            var today = DateTime.Today;
            modelBuilder.Entity<Attendance>().HasData(
                new Attendance
                {
                    Id = 1,
                    EmployeeId = 1,
                    Date = today.AddDays(-1),
                    CheckInTime = today.AddDays(-1).AddHours(8).AddMinutes(30),
                    CheckOutTime = today.AddDays(-1).AddHours(17).AddMinutes(30),
                    Status = "Present"
                },
                new Attendance
                {
                    Id = 2,
                    EmployeeId = 2,
                    Date = today.AddDays(-1),
                    CheckInTime = today.AddDays(-1).AddHours(9).AddMinutes(15),
                    CheckOutTime = today.AddDays(-1).AddHours(17).AddMinutes(45),
                    Status = "Late"
                },
                new Attendance
                {
                    Id = 3,
                    EmployeeId = 3,
                    Date = today.AddDays(-1),
                    CheckInTime = today.AddDays(-1).AddHours(8).AddMinutes(45),
                    CheckOutTime = today.AddDays(-1).AddHours(17).AddMinutes(15),
                    Status = "Present"
                }
            );
        }
    }
}