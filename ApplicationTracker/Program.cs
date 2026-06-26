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
                Console.WriteLine("3. Edit Application");
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

        static void EditApplication()
        {
            applications = repository.GetAllApplications();

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

            Console.WriteLine("What would you like to edit?");
            Console.WriteLine("1. Company Name");
            Console.WriteLine("2. Job Title");
            Console.WriteLine("3. Location");
            Console.WriteLine("4. Date Applied");
            Console.WriteLine("5. Status");
            Console.WriteLine("6. Notes");
            Console.WriteLine("Choose an option: ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.WriteLine("Enter new company name:");
                    string newCompanyName = Console.ReadLine();

                    repository.EditCompanyName(app.Id, newCompanyName);

                    Console.WriteLine("Application company name updated succesfully.");
                    break;
                case "2":
                    Console.WriteLine("Enter new job title: ");
                    string newJobTitle = Console.ReadLine();

                    repository.EditJobTitle(app.Id, newJobTitle);

                    Console.WriteLine("Application job title updated successfully.");
                    break;
                case "3":
                    Console.WriteLine("Enter new job location: ");
                    string newLocation = Console.ReadLine();

                    repository.EditJobLocation(app.Id, newLocation);

                    Console.WriteLine("Application job location updated successfully.");
                    break;
                case "4":
                    Console.WriteLine("Enter new date applied (yyyy-mm-dd): ");
                    string input = Console.ReadLine();
                    DateTime newDate = DateTime.Parse(input);

                    repository.EditDateApplied(app.Id, newDate);

                    Console.WriteLine("Application date applied updated successfully.");
                    break;
                case "5":
                    Console.WriteLine("Enter new status: ");
                    string newStatus = Console.ReadLine();

                    repository.EditApplicationStatus(app.Id, newStatus);

                    Console.WriteLine("Application status updated successfully.");
                    break;
                case "6":
                    Console.WriteLine("Enter new job notes: ");
                    string newJobNote = Console.ReadLine();

                    repository.EditJobNotes(app.Id, newJobNote);

                    Console.WriteLine("Application notes updated successfully.");
                    break;
                default:
                    Console.WriteLine("This edit option is not available.");
                    break;
            }
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
