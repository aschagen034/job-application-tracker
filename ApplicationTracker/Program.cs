
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
                Console.WriteLine("5. Exit");
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

            Console.Write("Job Status: ");
            app.JobStatus = Console.ReadLine();

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

            DisplayApplications();
        }

        // Allows the user to select an application and edit a selected field.
        static void EditApplication()
        {
            if (!LoadApplications())
            {
                return;
            }

            DisplayApplications();

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

                    Console.WriteLine("Application company name updated succesfully.");
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
                    Console.Write("Enter new status: ");
                    string newStatus = Console.ReadLine();

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

            DisplayApplications();

            Console.Write("Please select application to delete: ");
            Console.WriteLine();
            string num = Console.ReadLine();
            
            // Validate that the selected number exists in the displayed list.
            if (!int.TryParse(num, out int appNum) || appNum < 1 || appNum > applications.Count)
            {
                Console.WriteLine("Invalid application number.");
                return;
            }
            JobApplication app = applications[appNum - 1];
            // Delete application from the database.
            repository.DeleteApplication(app.Id);

            Console.WriteLine("Application successfully removed.");
        }

        // Prints all currently loaded applications in an easy-to-read format
        static void DisplayApplications()
        {
            for (int i = 0; i < applications.Count; i++)
            {
                JobApplication app = applications[i];

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
    }
}
