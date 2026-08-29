# Job Application Tracker

A C# console application for tracking job applications. The app allows users to add, view, edit, delete, and sort job applications, with data stored in a SQL Server database.

## Features

- Add new job applications
- View all saved applications
- Edit company name, job title, location, date applied, status, and notes
- Delete applications
- Filter applications by status
- Sort applications by date applied
- Export applications to a CSV file that can be opened in Excel
- Store application data in SQL Server
- Validate user input for dates, menu options, and application selections

## Technologies Used

- C#
- .NET
- SQL Server Express
- Microsoft.Data.SqlClient
- Git and GitHub

## Project Structure

- `Program.cs` - Handles the console menu, user input, validation, filtering, sorting, and display logic.
- `JobApplication.cs` - Represents a job application record.
- `ApplicationRepository.cs` - Handles all SQL Server database operations.

## What I Learned

- Building a console CRUD application in C#
- Connecting a C# application to SQL Server
- Using SQL commands with parameters
- Organizing code with a repository class
- Validating user input with TryParse
- Filtering and sorting data with LINQ
- Exporting application data to a CSV file