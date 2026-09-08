# Job Application Tracker

A C# console application for tracking job applications. The app allows users to add, view, edit, delete, filter, sort, and export job applications, with data stored in a SQL Server database.

## Features

- Add new job applications
- View all saved applications
- Edit existing application details
- Delete applications with confirmation
- Filter applications by status
- Sort applications by date applied
- Export applications to a CSV file that can be opened in Excel
- Store application data in SQL Server
- Validate user input for dates, menu options, and application selections


## Example

```text
=== Job Application Tracker ===
1. Add Application
2. View Applications
3. Edit Application
4. Delete Application
5. Filter by Application Status
6. Sort by Date Applied
7. Export Applications to CSV
8. Exit
Choose an option:
```

## Technologies Used

- C#
- .NET
- SQL Server Express
- Microsoft.Data.SqlClient
- Git and GitHub

## Prerequisites

Before running the project, install:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or SQL Server Express
- SQL Server Management Studio, Azure Data Studio, or another SQL client

## Getting Started

### 1. Clone the repository

```powershell
git clone <repository-url>
cd JobTracker
```

Replace `<repository-url>` with the URL of this repository.

### 2. Create the database

Connect to SQL Server and create a database named `JobTracker`:

```sql
CREATE DATABASE JobTracker;
```

### 3. Create the applications table

Run the following script against your SQL Server instance:

```text
sql/create_applications_table.sql
```

The script creates the `Applications` table used by the program.

An optional sample record can be inserted using:

```text
sql/add_application_one.sql
```

### 4. Configure the connection string

Open:

```text
ApplicationTracker/ApplicationTracker/Program.cs
```

Update the connection string so that the `Server` value matches your SQL Server instance:

```csharp
static string connectionString =
    "Server=YOUR_SERVER\\SQLEXPRESS;Database=JobTracker;Integrated Security=true;TrustServerCertificate=true;";
```

For example, a local SQL Server Express instance might use:

```csharp
static string connectionString =
    "Server=localhost\\SQLEXPRESS;Database=JobTracker;Integrated Security=true;TrustServerCertificate=true;";
```

> The current connection string may reference a machine-specific SQL Server instance and might need to be changed before the application can connect.

### 5. Restore the project

From the repository root, run:

```powershell
dotnet restore ApplicationTracker/ApplicationTracker.sln
```

### 6. Build the project

```powershell
dotnet build ApplicationTracker/ApplicationTracker.sln
```

### 7. Run the application

```powershell
dotnet run --project ApplicationTracker/ApplicationTracker/ApplicationTracker.csproj
```

## Application Statuses

Applications can use one of the following statuses:

- Applied
- Interviewing
- Offer
- Rejected

## CSV Export

The export option writes all application records to an `applications.csv` file on the current user's desktop.

The exported file includes:

- Company name
- Job title
- Job location
- Date applied
- Application status
- Notes

The file can be opened in Microsoft Excel or another spreadsheet application.

## Database Schema

The `Applications` table contains the following columns:

| Column | SQL type | Description |
|---|---|---|
| `Id` | `INT` | Unique application identifier |
| `CompanyName` | `NVARCHAR(100)` | Name of the company |
| `JobTitle` | `NVARCHAR(100)` | Title of the position |
| `JobLocation` | `NVARCHAR(100)` | Location of the position |
| `DateApplied` | `DATE` | Date the application was submitted |
| `JobStatus` | `NVARCHAR(50)` | Current application status |
| `Notes` | `NVARCHAR(255)` | Optional notes about the application |

## Project Structure

```text
JobTracker/
├── ApplicationTracker/
│   ├── ApplicationTracker/
│   │   ├── ApplicationRepository.cs
│   │   ├── ApplicationTracker.csproj
│   │   ├── JobApplication.cs
│   │   └── Program.cs
│   ├── ApplicationTracker.sln
│   └── README.md
└── sql/
    ├── add_application_one.sql
    └── create_applications_table.sql
```

- `Program.cs` handles the console menu, user input, validation, filtering, sorting, and display logic.
- `JobApplication.cs` defines the job application model.
- `ApplicationRepository.cs` handles SQL Server database operations.
- `create_applications_table.sql` creates the required database table.
- `add_application_one.sql` inserts an optional sample record.

## What I Learned

While building this project, I practiced:

- Creating a console-based CRUD application in C#
- Connecting a .NET application to SQL Server
- Executing parameterized SQL commands
- Organizing database operations with a repository class
- Validating console input
- Filtering and sorting collections with LINQ
- Handling nullable input and database values
- Exporting application data to CSV
- Organizing and documenting a C# project with Git and GitHub