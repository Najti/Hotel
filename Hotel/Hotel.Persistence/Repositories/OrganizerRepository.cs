using Hotel.Domain.Exceptions;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Hotel.Persistence.Repositories
{
    public class OrganizerRepository : IOrganizerRepository
    {
        private string connectionString;

        public OrganizerRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Organizer AddOrganizer(Organizer organizer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        // Controleer of het adres al bestaat op basis van adresdetails
                        string addressQuery = "SELECT Id FROM Address WHERE Street = @street AND City = @city AND PostalCode = @postalCode AND HouseNumber = @houseNumber;";
                        int addressId = -1;
                        using (SqlCommand checkAddressCmd = new SqlCommand(addressQuery, conn, transaction))
                        {
                            checkAddressCmd.Parameters.AddWithValue("@street", organizer.Contact.Address.Street);
                            checkAddressCmd.Parameters.AddWithValue("@city", organizer.Contact.Address.City);
                            checkAddressCmd.Parameters.AddWithValue("@postalCode", organizer.Contact.Address.PostalCode);
                            checkAddressCmd.Parameters.AddWithValue("@houseNumber", organizer.Contact.Address.HouseNumber);
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
                                    addAddressCmd.Parameters.AddWithValue("@street", organizer.Contact.Address.Street);
                                    addAddressCmd.Parameters.AddWithValue("@city", organizer.Contact.Address.City);
                                    addAddressCmd.Parameters.AddWithValue("@postalCode", organizer.Contact.Address.PostalCode);
                                    addAddressCmd.Parameters.AddWithValue("@houseNumber", organizer.Contact.Address.HouseNumber);

                                    addressId = Convert.ToInt32(addAddressCmd.ExecuteScalar());
                                }
                            }
                        }
                        string contactInfoQuery = "SELECT Id FROM ContactInfo WHERE Email = @email AND Phone = @phone;";
                        int contactInfoId = -1;
                        using (SqlCommand checkContactInfoCmd = new SqlCommand(contactInfoQuery, conn, transaction))
                        {
                            checkContactInfoCmd.Parameters.AddWithValue("@email", organizer.Contact.Email);
                            checkContactInfoCmd.Parameters.AddWithValue("@phone", organizer.Contact.Phone);
                            object existingContactInfoId = checkContactInfoCmd.ExecuteScalar();
                            if (existingContactInfoId != null)
                            {
                                contactInfoId = Convert.ToInt32(existingContactInfoId);
                            }
                            else
                            {  
                                string addContactInfoSql = "INSERT INTO ContactInfo(Email, Phone, AddressId) VALUES (@email, @phone, @addressId); SELECT SCOPE_IDENTITY();";
                                using (SqlCommand addContactInfoCmd = new SqlCommand(addContactInfoSql, conn, transaction))
                                {
                                    addContactInfoCmd.Parameters.AddWithValue("@email", organizer.Contact.Email);
                                    addContactInfoCmd.Parameters.AddWithValue("@phone", organizer.Contact.Phone);
                                    addContactInfoCmd.Parameters.AddWithValue("@addressId", addressId);

                                    contactInfoId = Convert.ToInt32(addContactInfoCmd.ExecuteScalar());
                                }
                            }
                        }

                        // Voeg de organizer toe met de contactinformatie en adres-ID's
                        string organizerSql = "INSERT INTO Organizer(Name, ContactInfoId, DeletedAt) VALUES (@name, @contactInfoId, @deletedAt); SELECT SCOPE_IDENTITY();";
                        using (SqlCommand organizerCmd = new SqlCommand(organizerSql, conn, transaction))
                        {
                            organizerCmd.Parameters.AddWithValue("@name", organizer.Name);
                            organizerCmd.Parameters.AddWithValue("@contactInfoId", contactInfoId); // Gebruik het addressId voor de ContactInfoId
                            organizerCmd.Parameters.AddWithValue("@deletedAt", DBNull.Value);

                            int organizerId = Convert.ToInt32(organizerCmd.ExecuteScalar());
                            organizer.Id = organizerId;
                        }

                        // Commit de transactie als alles goed is gegaan
                        transaction.Commit();
                    }
                }

                return organizer;
            }
            catch (Exception ex)
            {
                throw new OrganizerException("Error while adding organizer", ex);
            }
        }


        public void DeleteOrganizer(Organizer organizer)
        {
            try
            {
                string sql = "UPDATE Organizer SET DeletedAt = GETDATE() WHERE id = @id";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id", organizer.Id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new OrganizerException("Error while deleting organizer", ex);
            }
        }

        public Organizer GetOrganizer(int id)
        {
            try
            {
                string sql = "SELECT O.id AS OrganizerId, O.name AS OrganizerName, CI.id AS ContactInfoId, CI.Email, CI.Phone, A.Street, A.City, A.PostalCode, A.HouseNumber " +
                             "FROM Organizer O " +
                             "INNER JOIN ContactInfo CI ON O.ContactInfoId = CI.Id " +
                             "INNER JOIN Address A ON CI.AddressId = A.Id " +
                             "WHERE O.id = @id";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Address address = new Address((string)reader["Street"], (string)reader["City"], (string)reader["PostalCode"], (string)reader["HouseNumber"]);
                            ContactInfo contactInfo = new ContactInfo((string)reader["Email"], (string)reader["Phone"], address);
                            Organizer organizer = new Organizer((int)reader["OrganizerId"], (string)reader["OrganizerName"], contactInfo);
                            return organizer;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new OrganizerException("Error while getting organizer", ex);
            }
        }

        public List<Organizer> GetOrganizers(string filter)
        {
            try
            {
                List<Organizer> organizers = new List<Organizer>();
                string sql = "SELECT O.id AS OrganizerId, O.name AS OrganizerName, CI.id AS ContactInfoId, " +
                             "A.Street, A.City, A.PostalCode, A.HouseNumber, CI.Email, CI.Phone " +
                             "FROM Organizer O " +
                             "INNER JOIN ContactInfo CI ON O.ContactInfoId = CI.Id " +
                             "INNER JOIN Address A ON CI.AddressId = A.Id";

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    sql += " WHERE O.name LIKE @filter";
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;

                    if (!string.IsNullOrWhiteSpace(filter))
                    {
                        cmd.Parameters.AddWithValue("@filter", $"%{filter}%");
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Address address = new Address((string)reader["Street"], (string)reader["City"], (string)reader["PostalCode"], (string)reader["HouseNumber"]);
                            ContactInfo contactInfo = new ContactInfo((string)reader["Email"], (string)reader["Phone"], address);
                            Organizer organizer = new Organizer((int)reader["OrganizerId"], (string)reader["OrganizerName"], contactInfo);
                            organizers.Add(organizer);
                        }
                    }
                }
                return organizers;
            }
            catch (Exception ex)
            {
                throw new OrganizerException("Error while getting organizers", ex);
            }
        }


        public void UpdateOrganizer(Organizer organizer)
        {
            try
            {
                string sql = "UPDATE Organizer SET name = @name WHERE id = @id";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@name", organizer.Name);
                    cmd.Parameters.AddWithValue("@id", organizer.Id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new OrganizerException("Error while updating organizer", ex);
            }
        }
    }
}
