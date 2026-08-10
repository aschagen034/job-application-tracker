using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ApplicationTracker
{
    // Handles all SQL Server database operation for job applications.
    public class ApplicationRepository
    {
        private string connectionString;

        public ApplicationRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Gets all job applications from the database.
        public List<JobApplication> GetAllApplications()
        {
            List<JobApplication> applications = new List<JobApplication>();

            string query = @"
            SELECT Id, CompanyName, JobTitle, JobLocation, DateApplied, JobStatus, Notes
            FROM Applications;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        JobApplication app = new JobApplication();

                        app.Id = reader.GetInt32(0);
                        app.CompanyName = reader.GetString(1);
                        app.JobTitle = reader.GetString(2);
                        app.JobLocation = reader.GetString(3);
                        app.DateApplied = reader.GetDateTime(4);
                        app.JobStatus = reader.GetString(5);
                        app.Notes = reader.GetString(6);

                        applications.Add(app);
                    }
                }
            }

                return applications;
        }

        // Adds a new application to the database.
        public void AddApplication(JobApplication app)
        {
            string query = @"
            INSERT INTO Applications
                (CompanyName, JobTitle, JobLocation, DateApplied, JobStatus, Notes)
            VALUES
                (@CompanyName, @JobTitle, @JobLocation, @DateApplied, @JobStatus, @Notes);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CompanyName", app.CompanyName);
                command.Parameters.AddWithValue("@JobTitle", app.JobTitle);
                command.Parameters.AddWithValue("@JobLocation", app.JobLocation);
                command.Parameters.AddWithValue("@DateApplied", app.DateApplied);
                command.Parameters.AddWithValue("@JobStatus", app.JobStatus);
                command.Parameters.AddWithValue("@Notes", app.Notes);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();
            }

        }

        // Updates the company name for a specific job application.
        public void EditCompanyName(int id, string newCompanyName)
        {
            string query = @"
            UPDATE Applications
            SET CompanyName = @CompanyName
            WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CompanyName", newCompanyName);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    Console.WriteLine("No database row was updated.");
                }
            }
        }

        // Updates the job title for the specified application.
        public void EditJobTitle(int id, string newJobTitle)
        {
            string query = @"
            UPDATE Applications
            SET JobTitle = @JobTitle
            WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@JobTitle", newJobTitle);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    Console.WriteLine("No database row was updated.");
                }
            }
        }

        // Updates the job location for a specific job application.
        public void EditJobLocation(int id, string newJobLocation)
        {
            string query = @"
            UPDATE Applications
            SET JobLocation = @JobLocation
            WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@JobLocation", newJobLocation);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    Console.WriteLine("No database row was updated.");
                }
            }
        }

        //  Updates the date applied for a specific job application.
        public void EditDateApplied(int id, DateTime newDateApplied)
        {
            string query = @"
            UPDATE Applications
            SET DateApplied = @DateApplied
            WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DateApplied", newDateApplied);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    Console.WriteLine("No database row was updated.");
                }
            }
        }

        // Updates the status for a specific job application.
        public void EditApplicationStatus(int id, string newStatus)
        {
            string query = @"
            UPDATE Applications
            SET JobStatus = @JobStatus
            WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@JobStatus", newStatus);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    Console.WriteLine("No database row was updated.");
                }
            }
        }

        // Updates the notes for a specific job application.
        public void EditJobNotes(int id, string newJobNote)
        {
            string query = @"
            UPDATE Applications
            SET Notes = @Notes
            WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Notes", newJobNote);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    Console.WriteLine("No database row was updated.");
                }
            }
        }

        // Deletes a specific job application from the database using its Id.
        public void DeleteApplication(int id)
        {
            string query = @"
            DELETE FROM Applications
            WHERE Id = @Id;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    Console.WriteLine("No application was deleted.");
                }
            }
        }
    }
}
