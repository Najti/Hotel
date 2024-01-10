using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Persistence.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private string connectionString;
        private OrganizerRepository or;
        public ActivityRepository(string connectionString)
        {
            this.connectionString = connectionString;
            or = new OrganizerRepository(connectionString);
        }
        public void AddActivity(Activity activity)
        {
            string query = @"INSERT INTO Activity (Name, Description, Location, Startdate, Duration, AvailablePlaces, CostAdult, CostChild, Discount, OrganizerId)
                     VALUES (@Name, @Description, @Location, @Startdate, @Duration, @AvailablePlaces, @CostAdult, @CostChild, @Discount, @OrganizerId);
                     SELECT SCOPE_IDENTITY()";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", activity.Name);
                    command.Parameters.AddWithValue("@Description", activity.Description);
                    command.Parameters.AddWithValue("@Location", activity.Location);
                    command.Parameters.AddWithValue("@Startdate", activity.Date);
                    command.Parameters.AddWithValue("@Duration", activity.Duration);
                    command.Parameters.AddWithValue("@AvailablePlaces", activity.AvailablePlaces);
                    command.Parameters.AddWithValue("@CostAdult", activity.PriceAdult);
                    command.Parameters.AddWithValue("@CostChild", activity.PriceChild);
                    command.Parameters.AddWithValue("@Discount", activity.Discount);
                    command.Parameters.AddWithValue("@OrganizerId", activity.organizer.Id); 

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        activity.Id = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }



        public Activity GetActivityById(int id)
        {
            Activity activity = null;
            string query = "SELECT * FROM Activity WHERE Id = @Id";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader["Discount"] == DBNull.Value)
                        {
                            activity = new Activity((int)reader["Id"], (string)reader["Name"], (string)reader["Description"], (DateTime)reader["Startdate"], (int)reader["Duration"], (int)reader["AvailablePlaces"], (decimal)reader["CostAdult"], (decimal)reader["CostChild"], 0, (string)reader["Location"]);
                            activity.organizer = or.GetOrganizer((int)reader["OrganizerId"]);
                        }
                        else
                        {
                            activity = new Activity((int)reader["Id"], (string)reader["Name"], (string)reader["Description"], (DateTime)reader["Startdate"], (int)reader["Duration"], (int)reader["AvailablePlaces"], (decimal)reader["CostAdult"], (decimal)reader["CostChild"], (decimal)reader["Discount"], (string)reader["Location"]);
                            activity.organizer = or.GetOrganizer((int)reader["OrganizerId"]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return activity;
        }

        public List<Activity> GetActivities(string filter)
        {
            List<Activity> activities = new List<Activity>();

            string query = "SELECT * FROM Activity";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Activity activity;
                        if (reader["Discount"] == DBNull.Value)
                        {
                            activity = new Activity((int)reader["Id"], (string)reader["Name"], (string)reader["Description"], (DateTime)reader["Startdate"], (int)reader["Duration"], (int)reader["AvailablePlaces"], (decimal)reader["CostAdult"], (decimal)reader["CostChild"], 0, (string)reader["Location"]);
                            activity.organizer = or.GetOrganizer((int)reader["OrganizerId"]);
                        }
                        else
                        {
                            activity = new Activity((int)reader["Id"], (string)reader["Name"], (string)reader["Description"], (DateTime)reader["Startdate"], (int)reader["Duration"], (int)reader["AvailablePlaces"], (decimal)reader["CostAdult"], (decimal)reader["CostChild"], (decimal)reader["Discount"], (string)reader["Location"]);
                            activity.organizer = or.GetOrganizer((int)reader["OrganizerId"]);
                        }

                        activities.Add(activity);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return activities;
        }
    }
}

