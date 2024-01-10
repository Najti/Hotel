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
        private ActivityRepository ar;
        private CustomerRepository cr;

        public RegistrationRepository(string connectionString)
        {
            this.connectionString = connectionString;
            cr = new CustomerRepository(connectionString);
            ar = new ActivityRepository(connectionString);
        }
        public void AddRegistration(Registration registration)
        {
            string query = @"INSERT INTO Registration (activityid, customerid, numadults, numchildren, totalcost)
                     VALUES (@ActivityId, @CustomerId, @NumAdults, @NumChildren, @TotalCost);
                     SELECT SCOPE_IDENTITY()";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ActivityId", registration.Activity.Id);
                    command.Parameters.AddWithValue("@CustomerId", registration.Customer.Id);
                    command.Parameters.AddWithValue("@NumAdults", registration.NumberOfAdults);
                    command.Parameters.AddWithValue("@NumChildren", registration.NumberOfChildren);
                    command.Parameters.AddWithValue("@TotalCost", registration.Price);

                    connection.Open();
                    // ExecuteScalar is used to retrieve the single value returned by SCOPE_IDENTITY()
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        // Convert the retrieved value to the appropriate type (e.g., int)
                        registration.Id = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex; // Handle or log the exception as required
            }
        }


        public List<Registration> GetRegistrationsByCustomer(Customer customer)
        {
            List<Registration> registrations = new List<Registration>();

            string query = customer != null ? "SELECT * FROM Registration WHERE CustomerId = @CustomerId" : "SELECT * FROM Registration";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    if (customer != null)
                    {
                        command.Parameters.AddWithValue("@CustomerId", customer.Id);
                    }

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int activityId = (int)reader["activityid"];
                        int customerId = (int)reader["customerid"];

                        Customer registrationCustomer = customer != null ? customer : cr.GetCustomerByID(customerId);
                        Activity activity = ar.GetActivityById(activityId);
                        Registration registration = new Registration(registrationCustomer, activity)
                        {
                            Id = (int)reader["Id"],
                            Customer = registrationCustomer, // Use the retrieved or provided customer for this registration
                            Activity = activity,
                            Price = (decimal)reader["totalcost"],
                            NumberOfAdults = (int)reader["numadults"],
                            NumberOfChildren = (int)reader["numchildren"],
                            // Set other properties here based on database columns
                        };

                        registrations.Add(registration);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return registrations;
        }


    }
}