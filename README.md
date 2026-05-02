# HRMS — Leave Management System

ASP.NET Core 8 MVC | Entity Framework Core 8 | SQL Server

---

## Prerequisites

- .NET 8 SDK — https://dotnet.microsoft.com/download/dotnet/8.0
- SQL Server (Express or Full)

---

## How to Run

1. Extract the ZIP folder
2. Open `appsettings.json` and update the **Server** name to match your SQL Server instance
3. Run the following commands in terminal:
   ```
   dotnet restore
   dotnet run
   ```
4. Database and tables are **auto-created** on first run using EF Core Migrations
5. Open the URL shown in terminal 

---

## Default Admin Login

Admin account is **automatically created** through the **Database Seeder** (`Data/DbSeeder.cs`).  
No manual DB setup needed.

| Email | Password |
|---|---|
| admin@hrms.com | Admin@123 |

After logging in as Admin, you can create Employee accounts from the **Employees** page.

---

## Functionality

**Admin**
- Dashboard with summary (total employees, pending/approved/rejected counts)
- Create, Edit, Delete employees
- Approve or Reject leave requests with remarks
- Leave history of all employees
- Search and pagination on all listing pages

**Employee**
- Dashboard with leave balances and recent requests
- Apply for leave (Casual, Sick, Earned)
- Half-day leave support
- Cancel pending or approved leave (balance auto-restores)
- Leave history with search and pagination
- View leave balance

**General**
- Cookie-based authentication with role-based access (Admin / Employee)
- Global success/error popup notifications
- Instant search (AJAX, no page reload)
- Pagination on all listing pages
- Audit trail (created by, modified by, timestamps)
- Business day calculation (excludes weekends)
- Pending leave validation (prevents overbooking)

---

## Tech Stack

- ASP.NET Core 8 MVC
- Entity Framework Core 8 (Code First)
- SQL Server
- Bootstrap 5
- BCrypt (password hashing)
