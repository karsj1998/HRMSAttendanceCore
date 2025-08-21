# Employee Attendance System

A modern, responsive Employee Attendance Management System built with .NET Core 6.0 and Entity Framework Core.

## Features

### 🏠 Dashboard
- Real-time attendance statistics
- Today's attendance overview
- Quick action buttons
- Visual progress indicators

### 👥 Employee Management
- Add, edit, and delete employees
- Employee profile management
- Department and position tracking
- Active/Inactive status management

### 📅 Attendance Tracking
- **Weekly View**: Calendar-style weekly attendance display
- **Monthly View**: Full calendar view with detailed records
- Check-in and check-out time tracking
- Work hours calculation
- Status tracking (Present, Late, Absent, Half-day)

### 📊 Reports & Analytics
- Attendance summaries
- Work hours tracking
- Late arrival monitoring
- Absence tracking

## Technology Stack

- **Backend**: .NET Core 6.0
- **Database**: SQL Server (LocalDB)
- **ORM**: Entity Framework Core 6.0
- **Frontend**: HTML5, CSS3, JavaScript
- **UI Framework**: Bootstrap 5
- **Icons**: Font Awesome 6

## Prerequisites

- .NET Core 6.0 SDK or later
- SQL Server LocalDB (included with Visual Studio)
- Visual Studio 2022 or VS Code

## Installation & Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd EmployeeAttendance
```

### 2. Install Dependencies
```bash
dotnet restore
```

### 3. Database Setup
```bash
# Create the database and apply migrations
dotnet ef database update
```

### 4. Run the Application
```bash
dotnet run
```

### 5. Access the Application
Open your browser and navigate to: `https://localhost:5001` or `http://localhost:5000`

## Database Configuration

The application uses SQL Server LocalDB by default. You can modify the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EmployeeAttendance;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## Project Structure

```
EmployeeAttendance/
├── Controllers/          # MVC Controllers
│   ├── HomeController.cs
│   ├── EmployeesController.cs
│   └── AttendanceController.cs
├── Models/              # Data Models
│   ├── Employee.cs
│   └── Attendance.cs
├── Views/               # Razor Views
│   ├── Home/
│   ├── Employees/
│   └── Attendance/
├── Services/            # Business Logic
│   ├── IEmployeeService.cs
│   ├── EmployeeService.cs
│   ├── IAttendanceService.cs
│   └── AttendanceService.cs
├── Data/               # Data Access
│   └── ApplicationDbContext.cs
├── wwwroot/            # Static Files
│   ├── css/
│   └── js/
└── Program.cs          # Application Entry Point
```

## Key Features Explained

### Weekly Attendance View
- Select employee and week
- Navigate between weeks
- Visual status indicators
- Quick add/edit attendance records
- Summary statistics

### Monthly Attendance View
- Calendar-style layout
- Month and year selection
- Detailed attendance records
- Visual status badges
- Work hours calculation

### Employee Management
- Complete CRUD operations
- Department categorization
- Email validation
- Active status tracking

## Usage Guide

### Adding an Employee
1. Navigate to Employees → Add New Employee
2. Fill in employee details
3. Select department and position
4. Set hire date and active status
5. Click "Create Employee"

### Recording Attendance
1. Go to Attendance → Add Attendance
2. Select employee and date
3. Enter check-in and check-out times
4. Set attendance status
5. Add optional notes
6. Click "Create Attendance"

### Viewing Weekly Attendance
1. Navigate to Attendance → Weekly View
2. Select employee from dropdown
3. Choose week start date
4. View attendance records
5. Use navigation buttons to change weeks

### Viewing Monthly Attendance
1. Go to Attendance → Monthly View
2. Select employee, month, and year
3. View calendar-style layout
4. Click on dates to add/edit records
5. View detailed table below calendar

## Customization

### Adding New Departments
Edit the `Create.cshtml` view in the Employees folder and add new options to the department select list.

### Modifying Work Hours
Update the `IsLate` property in the `Attendance.cs` model to change the expected check-in time (currently set to 9 AM).

### Styling Changes
Modify `wwwroot/css/site.css` to customize the appearance of the application.

## Troubleshooting

### Database Connection Issues
- Ensure SQL Server LocalDB is installed
- Check connection string in `appsettings.json`
- Run `dotnet ef database update` to create database

### Migration Issues
```bash
# Remove existing migrations
dotnet ef migrations remove

# Add new migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### Build Errors
- Ensure .NET Core 6.0 SDK is installed
- Run `dotnet restore` to restore packages
- Check for any missing dependencies

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support and questions, please open an issue in the repository or contact the development team.

---

**Note**: This is a demo application with sample data. For production use, ensure proper security measures, data validation, and backup procedures are implemented.