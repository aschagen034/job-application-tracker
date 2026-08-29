using System.Linq;
using System.IO;

namespace ApplicationTracker
{
    internal class Program
    {
        // Stores the applications currently loaded from the database.
        static List<JobApplication> applications = new List<JobApplication>();

        // Connection string used to connect to the JobTracker SQL Server Database.
        static string connectionString =
            "Server=ORION\\SQLEXPRESS;Database=JobTracker;Integrated Security=true;TrustServerCertificate=true;";

        // Repository handles all database operations for job applications
        static ApplicationRepository repository =
            new ApplicationRepository(connectionString);

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== Job Application Tracker ===");
                Console.WriteLine("1. Add Application");
                Console.WriteLine("2. View Applications");
                Console.WriteLine("3. Edit Application");
                Console.WriteLine("4. Delete Application");
                Console.WriteLine("5. Filter by Application Status");
                Console.WriteLine("6. Sort by Date Applied");
                Console.WriteLine("7. Export Applications to CSV");
                Console.WriteLine("8. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddApplication();
                        break;
                    case "2":
                        ViewApplications();
                        break;
                    case "3":
                        EditApplication();
                        break;
                    case "4":
                        DeleteApplication();
                        break;
                    case "5":
                        FilterApplicationByStatus();
                        break;
                    case "6":
                        SortApplicationsByDate();
                        break;
                    case "7":
                        ExportApplicationsToCsv();
                        break;
                    case "8":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
       
        }
        // Collects application information from the user
        // Saves the new application to the database
        static void AddApplication()
        {
            JobApplication app = new JobApplication();

            Console.Write("Company Name: ");
            app.CompanyName = Console.ReadLine();

            Console.Write("Job Title: ");
            app.JobTitle = Console.ReadLine();

            Console.Write("Job Location: ");
            app.JobLocation = Console.ReadLine();

            DateTime dateApplied;

            while (true)
            {
                Console.Write("Date Applied (yyyy-mm-dd): ");
                string dateInput = Console.ReadLine();

                if (DateTime.TryParse(dateInput, out dateApplied))
                {
                    break;
                }

                Console.WriteLine("Please enter a valid date.");
            }

            app.DateApplied = dateApplied;

            app.JobStatus = ChooseStatus("Choose application status: ");

            Console.Write("Notes: ");
            app.Notes = Console.ReadLine();

            //Send the completed application to the repository to be inserted into SQL.
            repository.AddApplication(app);

            Console.WriteLine("Application added successfully");
        }

        // Loads and displays all applications stored in the database.
        static void ViewApplications()
        {
            // Stop if there are no applications to display.
            if (!LoadApplications())
            {
                return;
            }

            DisplayApplications(applications);
        }

        // Allows the user to select an application and edit a selected field.
        static void EditApplication()
        {
            if (!LoadApplications())
            {
                return;
            }

            DisplayApplications(applications);

            Console.Write("Please select application to update: ");
            string num = Console.ReadLine();
            
            // Checks to see if user enter a valid application number.
            if (!int.TryParse(num, out int appNum) || appNum < 1 || appNum > applications.Count)
            {
                Console.WriteLine("Invalid application number.");
                return;
            }

            JobApplication app = applications[appNum - 1];

            Console.WriteLine("What would you like to edit?");
            Console.WriteLine("1. Company Name");
            Console.WriteLine("2. Job Title");
            Console.WriteLine("3. Location");
            Console.WriteLine("4. Date Applied");
            Console.WriteLine("5. Status");
            Console.WriteLine("6. Notes");
            Console.Write("Choose an option: ");

            string option = Console.ReadLine();

            // Update only the field selected by the user.
            switch (option)
            {
                case "1":
                    Console.Write("Enter new company name:");
                    string newCompanyName = Console.ReadLine();

                    repository.EditCompanyName(app.Id, newCompanyName);

                    Console.WriteLine("Application company name updated successfully.");
                    break;

                case "2":
                    Console.Write("Enter new job title: ");
                    string newJobTitle = Console.ReadLine();

                    repository.EditJobTitle(app.Id, newJobTitle);

                    Console.WriteLine("Application job title updated successfully.");
                    break;

                case "3":
                    Console.Write("Enter new job location: ");
                    string newLocation = Console.ReadLine();

                    repository.EditJobLocation(app.Id, newLocation);

                    Console.WriteLine("Application job location updated successfully.");
                    break;

                case "4":
                    DateTime newDate;
                    // Loop until user enters a valid date
                    while (true)
                    {

                        Console.Write("Enter new date applied (yyyy-mm-dd): ");
                        string input = Console.ReadLine();

                        if (DateTime.TryParse(input, out newDate))
                        {
                            break;
                        }

                        Console.WriteLine("Please enter a valid date.");
                    }

                    repository.EditDateApplied(app.Id, newDate);

                    Console.WriteLine("Application date applied updated successfully.");
                    break;

                case "5":             
                    string newStatus = ChooseStatus("Choose new application status: ");

                    repository.EditApplicationStatus(app.Id, newStatus);

                    Console.WriteLine("Application status updated successfully.");
                    break;

                case "6":
                    Console.Write("Enter new job notes: ");
                    string newJobNote = Console.ReadLine();

                    repository.EditJobNotes(app.Id, newJobNote);

                    Console.WriteLine("Application notes updated successfully.");
                    break;

                default:
                    Console.WriteLine("This edit option is not available.");
                    break;
            }
        }

        // Allows the user to select an application and remove it from the database.
        static void DeleteApplication()
        {
            if (!LoadApplications())
            {
                return;
            }

            DisplayApplications(applications);

            Console.Write("Please select application to delete: ");
            string num = Console.ReadLine();

            // Validate that the selected number exists in the displayed list.
            if (!int.TryParse(num, out int appNum) || appNum < 1 || appNum > applications.Count)
            {
                Console.WriteLine("Invalid application number.");
                return;
            }

            JobApplication app = applications[appNum - 1];

            Console.Write($"Are you sure you want to delete {app.CompanyName} - {app.JobTitle}? (yes/no): ");
            string choice = Console.ReadLine();

            if (choice.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
            choice.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                // Delete application from the database.
                repository.DeleteApplication(app.Id);

                Console.WriteLine("Application successfully removed.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Delete cancelled.");
            }

        }

        // Prints all currently loaded applications in an easy-to-read format
        static void DisplayApplications(List<JobApplication> applicationsToDisplay)
        {
            for (int i = 0; i < applicationsToDisplay.Count; i++)
            {
                JobApplication app = applicationsToDisplay[i];

                Console.WriteLine("\n-----------------------------");
                Console.WriteLine($"{i + 1})");
                Console.WriteLine($"Company: {app.CompanyName}");
                Console.WriteLine($"Title: {app.JobTitle}");
                Console.WriteLine($"Location: {app.JobLocation}");
                Console.WriteLine($"Date Applied: {app.DateApplied.ToShortDateString()}");
                Console.WriteLine($"Job Status: {app.JobStatus}");
                Console.WriteLine($"Notes: {app.Notes}");
            }
        }

        // Loads the latest applications from the database and returns false if none exist.
        static bool LoadApplications()
        {
            applications = repository.GetAllApplications();

            if (applications.Count == 0)
            {
                Console.WriteLine("No applications found.");
                return false;
            }

            return true;
        }

        // Filters and displays applications that match the status entered by the user.
        static void FilterApplicationByStatus()
        {
            if (!LoadApplications())
            {
                return;
            }

            string status = ChooseStatus("Choose status to filter by: ");

            List<JobApplication> filteredApplications = applications
                .Where(app => app.JobStatus.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (filteredApplications.Count == 0 )
            {
                Console.WriteLine("No applications found with this status.");
                return;
            }

            DisplayApplications( filteredApplications );
        }

        // Displays a list of valid job status options and returns the selected status.
        static string ChooseStatus(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                Console.WriteLine("1. Applied");
                Console.WriteLine("2. Interviewing");
                Console.WriteLine("3. Offer");
                Console.WriteLine("4. Rejected");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        return "Applied";
                    case "2":
                        return "Interviewing";
                    case "3":
                        return "Offer";
                    case "4":
                        return "Rejected";
                    default:
                        Console.WriteLine("Invalid status choice. Please try again.");
                        break;
                }
            }
        }

        // Loads applications from the database and displays them sorted by date applied
        static void SortApplicationsByDate()
        {
            if (!LoadApplications())
            {
                return;
            }

            Console.WriteLine("1. Newest first");
            Console.WriteLine("2. Oldest first");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            List<JobApplication> sortedApplications;

            switch (choice)
            {
                case "1":
                    sortedApplications = applications.OrderByDescending(app => app.DateApplied).ToList();
                    break;
                case "2":
                    sortedApplications = applications.OrderBy(app => app.DateApplied).ToList();
                    break;
                default:
                    Console.WriteLine("Invalid sort option.");
                    return;
            }

            DisplayApplications(sortedApplications);
        }

        // Exports all saved applications to a CSV file that can be opened in Excel.
        static void ExportApplicationsToCsv()
        {
            if (!LoadApplications())
            {
                return;
            }

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "applications.csv");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Company Name,Job Title,Job Location,Date Applied,Job Status,Notes");

                foreach (JobApplication app in applications)
                {
                    writer.WriteLine(
                        $"{FormatCsvValue(app.CompanyName)}," +
                        $"{FormatCsvValue(app.JobTitle)}," +
                        $"{FormatCsvValue(app.JobLocation)}," +
                        $"{app.DateApplied.ToShortDateString()}," +
                        $"{FormatCsvValue(app.JobStatus)}," +
                        $"{FormatCsvValue(app.Notes)}"
                    );
                }
            }

            Console.WriteLine($"Applications exported successfully to {filePath}");
        }

        // Formats a value so it can be safely written to a CSV file.
        static string FormatCsvValue(string value)
        {
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }

            return value;
        }
    }
}
