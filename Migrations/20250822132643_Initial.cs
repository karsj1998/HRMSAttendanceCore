using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAttendance.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payrolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Allowances = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deductions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payrolls_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Department", "Email", "EmployeeId", "FirstName", "HireDate", "IsActive", "LastName", "Position" },
                values: new object[] { 1, "IT", "john.doe@company.com", "EMP001", "John", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Doe", "Software Developer" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Department", "Email", "EmployeeId", "FirstName", "HireDate", "IsActive", "LastName", "Position" },
                values: new object[] { 2, "HR", "jane.smith@company.com", "EMP002", "Jane", new DateTime(2022, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Smith", "HR Manager" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Department", "Email", "EmployeeId", "FirstName", "HireDate", "IsActive", "LastName", "Position" },
                values: new object[] { 3, "Finance", "mike.johnson@company.com", "EMP003", "Mike", new DateTime(2023, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Johnson", "Accountant" });

            migrationBuilder.InsertData(
                table: "Attendances",
                columns: new[] { "Id", "CheckInTime", "CheckOutTime", "Date", "EmployeeId", "Notes", "Status" },
                values: new object[] { 1, new DateTime(2025, 8, 21, 8, 30, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 21, 17, 30, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 21, 0, 0, 0, 0, DateTimeKind.Local), 1, null, "Present" });

            migrationBuilder.InsertData(
                table: "Attendances",
                columns: new[] { "Id", "CheckInTime", "CheckOutTime", "Date", "EmployeeId", "Notes", "Status" },
                values: new object[] { 2, new DateTime(2025, 8, 21, 9, 15, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 21, 17, 45, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 21, 0, 0, 0, 0, DateTimeKind.Local), 2, null, "Late" });

            migrationBuilder.InsertData(
                table: "Attendances",
                columns: new[] { "Id", "CheckInTime", "CheckOutTime", "Date", "EmployeeId", "Notes", "Status" },
                values: new object[] { 3, new DateTime(2025, 8, 21, 8, 45, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 21, 17, 15, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 21, 0, 0, 0, 0, DateTimeKind.Local), 3, null, "Present" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeId_Date",
                table: "Attendances",
                columns: new[] { "EmployeeId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeId",
                table: "Employees",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_EmployeeId_Year_Month",
                table: "Payrolls",
                columns: new[] { "EmployeeId", "Year", "Month" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Payrolls");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
