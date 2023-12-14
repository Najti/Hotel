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
    public class MemberRepository : IMemberRepository
    {
        private string connectionString;
        private CustomerRepository crepo;
        public MemberRepository(string connectionString)
        {
            this.connectionString = connectionString;
            crepo = new CustomerRepository(connectionString);
        }
        public void AddMember(Member member)
        {
            try
            {
                string sql = "INSERT INTO Member(customerId, name, birthday, status) VALUES (@customerId, @name, @birthday, @status)";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@customerId", member.Customer.Id); // Vervang dit door de juiste customerId
                    cmd.Parameters.AddWithValue("@name", member.Name);
                    cmd.Parameters.AddWithValue("@birthday", member.Birthday);
                    cmd.Parameters.AddWithValue("@status", 1); // Of gebruik de juiste statuswaarde

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new MemberException("Error while adding member", ex);
            }
        }

        public void DeleteMember(Member member)
        {
            try
            {
                string sql = "DELETE FROM Member WHERE memberId = @memberId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@memberId", member.Customer.Id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new MemberException("Error while deleting member", ex);
            }
        }

        public Member GetMemberByID(int memberId)
        {
            try
            {
                string sql = "SELECT memberId, customerId, name, birthday FROM Member WHERE memberId = @memberId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@memberId", memberId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Customer c = crepo.GetCustomerByID((int)reader["customerId"]);
                            Member member = new Member((int)reader["memberId"], c, (string)reader["name"], (DateTime)reader["birthday"]);
                            return member;
                        }
                    }
                }
                return null; // Return null als er geen lid wordt gevonden met het opgegeven ID
            }
            catch (Exception ex)
            {
                throw new MemberException("Error while getting member by ID", ex);
            }
        }

        public List<Member> GetMembers(string filter)
        {
            try
            {
                List<Member> members = new List<Member>();
                string sql = "SELECT memberId, customerId, name, birthday FROM Member";

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    sql += " WHERE name LIKE @filter";
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
                            
                            Customer c = crepo.GetCustomerByID((int)reader["customerId"]);
                            Member member = new Member((int)reader["memberId"],c , (string)reader["name"], (DateTime)reader["birthday"]);
                            members.Add(member);
                        }
                    }
                }
                return members;
            }
            catch (Exception ex)
            {
                throw new MemberException("Error while getting members", ex);
            }
        }

        public void UpdateMember(Member member)
        {
            try
            {
                string sql = "UPDATE Member SET customerId = @customerId, name = @name, birthday = @birthday WHERE memberId = @memberId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@customerId", member.Customer.Id);
                    cmd.Parameters.AddWithValue("@name", member.Name);
                    cmd.Parameters.AddWithValue("@birthday", member.Birthday);
                    cmd.Parameters.AddWithValue("@memberId", member.Id);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new MemberException("Error while updating member", ex);
            }
        }

    }
}
