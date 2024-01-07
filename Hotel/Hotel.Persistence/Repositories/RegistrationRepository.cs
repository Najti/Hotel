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
    public class RegistrationRepository : IRegistrationRepository
    {
        private string connectionString;

        public RegistrationRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void AddRegistration(Registration registration)
        {
            throw new NotImplementedException();
        }
        //public List<Activity> GetActivitiesByCustomer(string filter, Customer customer)
        //{
        //    //    List<Activity> activities = new List<Activity>();

        //    //    string query = "SELECT * FROM Activity";
        //    //    try
        //    //    {
        //    //        using (SqlConnection connection = new SqlConnection(connectionString))
        //    //        {
        //    //            SqlCommand command = new SqlCommand(query, connection);
        //    //            connection.Open();

        //    //            SqlDataReader reader = command.ExecuteReader();
        //    //            while (reader.Read())
        //    //            {
        //    //                Activity activity;
        //    //                if (reader["Discount"] == DBNull.Value)
        //    //                {
        //    //                    activity = new Activity((int)reader["Id"], (string)reader["Name"], (string)reader["Description"], (DateTime)reader["Startdate"], (int)reader["Duration"], (int)reader["AvailablePlaces"], (decimal)reader["CostAdult"], (decimal)reader["CostChild"], 0, (string)reader["Location"]);

        //    //                }
        //    //                else
        //    //                {
        //    //                    activity = new Activity((int)reader["Id"], (string)reader["Name"], (string)reader["Description"], (DateTime)reader["Startdate"], (int)reader["Duration"], (int)reader["AvailablePlaces"], (decimal)reader["CostAdult"], (decimal)reader["CostChild"], (decimal)reader["Discount"], (string)reader["Location"]);
        //    //                }

        //    //                activities.Add(activity);
        //    //            }
        //    //        }
        //    //    }
        //    //    catch (SqlException ex)
        //    //    {
        //    //        throw ex;
        //    //    }

        //    //    return activities;
        //    //}
        //}
    }
}