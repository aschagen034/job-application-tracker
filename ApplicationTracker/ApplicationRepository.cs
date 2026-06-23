using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ApplicationTracker
{
    public class ApplicationRepository
    {
        private string connectionString;

        public ApplicationRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

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
        public void UpdateApplicationStatus(int id, string newStatus)
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
