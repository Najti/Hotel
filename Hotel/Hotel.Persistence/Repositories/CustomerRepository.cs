using Hotel.Domain.Exceptions;
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
    public class CustomerRepository : ICustomerRepository
    {
        private string connectionString;

        public CustomerRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Customer> GetCustomers(string filter)
        {
            try
            {
                Dictionary<int, Customer> customers = new Dictionary<int, Customer>();
                string sql = "SELECT t1.id, t1.name customername, t2.email, t2.phone, " +
                             "CONCAT(t3.Street, '|', t3.City, '|', t3.PostalCode, '|', t3.HouseNumber) as Address, " +
                             "t4.name membername, t4.birthday " +
                             "FROM Customer t1 " +
                             "LEFT JOIN ContactInfo t2 ON t1.ContactInfoID = t2.Id " +
                             "LEFT JOIN Address t3 ON t2.AddressId = t3.Id " +
                             "LEFT JOIN (SELECT * FROM member) t4 ON t1.id = t4.customerId " +
                             "WHERE t1.DeletedAt IS NULL";

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    sql += " AND (t1.name LIKE @filter OR t2.email LIKE @filter)";
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    if (!string.IsNullOrWhiteSpace(filter))
                        cmd.Parameters.AddWithValue("@filter", $"%{filter}%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["ID"]);
                            if (!customers.ContainsKey(id))
                            {
                                Customer customer = new Customer(id, (string)reader["customername"], new ContactInfo((string)reader["email"], (string)reader["phone"], new Address((string)reader["address"])));
                                customers.Add(id, customer);
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("membername")))
                            {
                                Member member = new Member((string)reader["membername"], (DateTime)reader["birthday"]);
                                customers[id].AddMember(member);
                            }
                        }
                    }
                }
                return customers.Values.ToList();
            }
            catch (Exception ex)
            {
                throw new CustomerException("Error while getting customers", ex);
            }
        }
        public void AddCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        // Controleer of de ContactInfo al bestaat op basis van email en telefoon
                        string contactInfoQuery = "SELECT Id FROM ContactInfo WHERE Email = @email AND Phone = @phone;";
                        int contactInfoId = -1;
                        using (SqlCommand checkContactInfoCmd = new SqlCommand(contactInfoQuery, conn, transaction))
                        {
                            checkContactInfoCmd.Parameters.AddWithValue("@email", customer.Contact.Email);
                            checkContactInfoCmd.Parameters.AddWithValue("@phone", customer.Contact.Phone);
                            object existingContactInfoId = checkContactInfoCmd.ExecuteScalar();
                            if (existingContactInfoId != null)
                            {
                                contactInfoId = Convert.ToInt32(existingContactInfoId);
                            }
                            else
                            {
                                // Voeg een nieuwe ContactInfo toe
                                //ERROR HIER ERGENS
                                string addContactInfoSql = "INSERT INTO ContactInfo(Email, Phone, AddressId) VALUES (@email, @phone, @addressId); SELECT SCOPE_IDENTITY();";
                                using (SqlCommand addContactInfoCmd = new SqlCommand(addContactInfoSql, conn, transaction))
                                {
                                    addContactInfoCmd.Parameters.AddWithValue("@email", customer.Contact.Email);
                                    addContactInfoCmd.Parameters.AddWithValue("@phone", customer.Contact.Phone);
                                    addContactInfoCmd.Parameters.AddWithValue("@addressId", customer.Contact.Address.Id);

                                    contactInfoId = Convert.ToInt32(addContactInfoCmd.ExecuteScalar());
                                }
                            }
                        }

                        // Controleer of het adres al bestaat op basis van adresdetails
                        string addressQuery = "SELECT Id FROM Address WHERE Street = @street AND City = @city AND PostalCode = @postalCode AND HouseNumber = @houseNumber;";
                        int addressId = -1;
                        using (SqlCommand checkAddressCmd = new SqlCommand(addressQuery, conn, transaction))
                        {
                            checkAddressCmd.Parameters.AddWithValue("@street", customer.Contact.Address.Street);
                            checkAddressCmd.Parameters.AddWithValue("@city", customer.Contact.Address.City);
                            checkAddressCmd.Parameters.AddWithValue("@postalCode", customer.Contact.Address.PostalCode);
                            checkAddressCmd.Parameters.AddWithValue("@houseNumber", customer.Contact.Address.HouseNumber);
                            object existingAddressId = checkAddressCmd.ExecuteScalar();
                            if (existingAddressId != null)
                            {
                                addressId = Convert.ToInt32(existingAddressId);
                            }
                            else
                            {
                                // Voeg een nieuw adres toe
                                string addAddressSql = "INSERT INTO Address(Street, City, PostalCode, HouseNumber) VALUES (@street, @city, @postalCode, @houseNumber); SELECT SCOPE_IDENTITY();";
                                using (SqlCommand addAddressCmd = new SqlCommand(addAddressSql, conn, transaction))
                                {
                                    addAddressCmd.Parameters.AddWithValue("@street", customer.Contact.Address.Street);
                                    addAddressCmd.Parameters.AddWithValue("@city", customer.Contact.Address.City);
                                    addAddressCmd.Parameters.AddWithValue("@postalCode", customer.Contact.Address.PostalCode);
                                    addAddressCmd.Parameters.AddWithValue("@houseNumber", customer.Contact.Address.HouseNumber);

                                    addressId = Convert.ToInt32(addAddressCmd.ExecuteScalar());
                                }
                            }
                        }

                        // Voeg de klant toe met de contactinformatie en adres-ID's
                        string customerSql = "INSERT INTO Customer(Name, ContactInfoID) VALUES (@name, @contactInfoId); SELECT SCOPE_IDENTITY();";
                        using (SqlCommand customerCmd = new SqlCommand(customerSql, conn, transaction))
                        {
                            customerCmd.Parameters.AddWithValue("@name", customer.Name);
                            customerCmd.Parameters.AddWithValue("@contactInfoId", contactInfoId);

                            int customerId = Convert.ToInt32(customerCmd.ExecuteScalar());
                            customer.Id = customerId;

                            // Voeg leden toe aan de klant
                            foreach (Member member in customer.GetMembers())
                            {
                                string memberSql = "INSERT INTO Member(CustomerId, Name, Birthday) VALUES (@customerId, @name, @birthday)";
                                using (SqlCommand memberCmd = new SqlCommand(memberSql, conn, transaction))
                                {
                                    memberCmd.Parameters.AddWithValue("@customerId", customerId);
                                    memberCmd.Parameters.AddWithValue("@name", member.Name);
                                    memberCmd.Parameters.AddWithValue("@birthday", member.Birthday);
                                    memberCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // Commit de transactie als alles goed is gegaan
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomerException("Error while adding customer", ex);
            }
        }

        public Customer GetCustomerByID(int id)
        {
            try
            {
                string sql = "SELECT c.id, c.name as customername, ci.email, ci.phone, " +
                             "CONCAT(a.Street, '|', a.City, '|', a.PostalCode, '|', a.HouseNumber) as fullAddress, " +
                             "m.name as membername, m.birthday " +
                             "FROM Customer c " +
                             "LEFT JOIN Member m ON c.id = m.customerId " +
                             "LEFT JOIN ContactInfo ci ON c.ContactInfoID = ci.Id " +
                             "LEFT JOIN Address a ON ci.AddressId = a.Id " +
                             "WHERE c.id = @customerId AND c.DeletedAt IS NULL";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@customerId", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Customer customer = null;
                        while (reader.Read())
                        {
                            if (customer == null)
                            {
                                string email = reader["email"] != DBNull.Value ? (string)reader["email"] : string.Empty;
                                string phone = reader["phone"] != DBNull.Value ? (string)reader["phone"] : string.Empty;
                                string fullAddress = reader["fullAddress"] != DBNull.Value ? (string)reader["fullAddress"] : string.Empty;

                                customer = new Customer(id, (string)reader["customername"], new ContactInfo(email, phone, new Address(fullAddress)));
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("membername")))
                            {
                                Member member = new Member((string)reader["membername"], (DateTime)reader["birthday"]);
                                customer.AddMember(member);
                            }
                        }
                        return customer;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomerException("Error while getting customer by ID", ex);
            }
        }


        public Customer UpdateCustomer(Customer customer)
        {
            try
            {
                string sql = "UPDATE Customer SET name = @name, email = @email, " +
                             "phone = @phone, address = @address WHERE id = @customerId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@name", customer.Name);
                    cmd.Parameters.AddWithValue("@email", customer.Contact.Email);
                    cmd.Parameters.AddWithValue("@phone", customer.Contact.Phone);
                    cmd.Parameters.AddWithValue("@address", customer.Contact.Address.ToAddressLine());
                    cmd.Parameters.AddWithValue("@customerId", customer.Id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new CustomerException("Error while updating customer, check if user exists");
                    }

                    return customer;
                }
            }
            catch (Exception ex)
            {
                throw new CustomerException("Error while updating customer", ex);
            }
        }

        public void DeleteCustomer(Customer customer)
        {
            try
            {
                string sql = "UPDATE Customer SET DeletedAt = GETDATE() WHERE id = @customerId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@customerId", customer.Id);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new CustomerException("Error while deleting customer, check if user exists");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CustomerException("Error while deleting customer", ex);
            }
        }
    }
}
