using System.Reflection.Metadata.Ecma335;

namespace ApplicationTracker
{
    internal class Program
    {
        static List<JobApplication> applications = new List<JobApplication>();

        static string connectionString =
            "Server=ORION\\SQLEXPRESS;Database=JobTracker;Integrated Security=true;TrustServerCertificate=true;";

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
                Console.WriteLine("3. Update Application Status");
                Console.WriteLine("4. Delete Application");
                Console.WriteLine("5. Exit");
                Console.WriteLine("Choose an option: ");

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
                        UpdateApplicationStatus();
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

        static void AddApplication()
        {

            JobApplication app = new JobApplication();

            Console.Write("Company Name: ");
            app.CompanyName = Console.ReadLine();

            Console.Write("Job Title: ");
            app.JobTitle = Console.ReadLine();

            Console.Write("Job Location: ");
            app.JobLocation = Console.ReadLine();

            Console.Write("Date Applied (yyyy-mm-dd): ");
            app.DateApplied = DateTime.Parse(Console.ReadLine());

            Console.Write("Job Status: ");
            app.JobStatus = Console.ReadLine();

            Console.Write("Notes: ");
            app.Notes = Console.ReadLine();

            repository.AddApplication(app);

            Console.WriteLine("Application added successfully");


        }

        static void ViewApplications()
        {
            applications = repository.GetAllApplications();

            if (applications.Count == 0)
            {
                Console.WriteLine("No applications found.");
                return;
            }

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

        static void UpdateApplicationStatus()
        {
            if (applications.Count == 0)
            {
                Console.WriteLine("No applications found.");
                return;
            }

            ViewApplications();

            Console.WriteLine("Please select application to update: ");
            string num = Console.ReadLine();
            int appNum = int.Parse(num);

            if (appNum <  1 || appNum > applications.Count)
            {
                Console.WriteLine("Invalid application number.");
                return;
            }
            JobApplication app = applications[appNum - 1];

            Console.WriteLine("Enter new status: ");
            string newStatus = Console.ReadLine();

            repository.UpdateApplicationStatus(app.Id, newStatus);

            Console.WriteLine("Application status updated successfully.");
     
        }

        static void DeleteApplication()
        {
            if (applications.Count == 0)
            {
                Console.WriteLine("No applications found.");
                return;
            }

            ViewApplications();

            Console.WriteLine("Please select application to delete: ");
            string num = Console.ReadLine();
            int appNum = int.Parse(num);

            if (appNum < 1 || appNum > applications.Count)
            {
                Console.WriteLine("Invalid application number.");
                return;
            }

            JobApplication app = applications[appNum - 1];
            repository.DeleteApplication(app.Id);

            Console.WriteLine("Application successfully removed.");

            
        }
    }
}
