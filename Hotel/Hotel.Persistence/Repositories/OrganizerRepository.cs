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
                string sql = "INSERT INTO Organizer(name, ContactInfoId, DeletedAt) VALUES (@name, @contactInfoId, @deletedAt)";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@name", organizer.Name);
                    cmd.Parameters.AddWithValue("@contactInfoId", organizer.ContactInfo.Id);
                    cmd.Parameters.AddWithValue("@deletedAt", DBNull.Value);

                    cmd.ExecuteNonQuery();
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
                string sql = "SELECT O.id AS OrganizerId, O.name AS OrganizerName, CI.id AS ContactInfoId, A.Street, A.City, A.PostalCode, A.HouseNumber " +
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
