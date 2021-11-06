using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TenmoServer.Models;
using TenmoServer.Security;

namespace TenmoServer.DAO
{
    public class UserSqlDAO : IUserDAO
    {
        private readonly string connectionString;
        const decimal startingBalance = 1000;

        public UserSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        /// <summary>
        /// Gets the user details of the user attempting to log in
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUser(string username)
        {
            User returnUser = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT user_id, username, password_hash, salt FROM users WHERE username = @username", conn);
                cmd.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    returnUser = GetUserFromReader(reader);
                }
            }

            return returnUser;
        }
        /// <summary>
        /// Gets the username, account ID, and user ID of all users in the system
        /// </summary>
        /// <returns></returns>
        public List<ReturnUser> GetUsers()
        {
            List<ReturnUser> returnUsers = new List<ReturnUser>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT username, a.account_id, u.user_id FROM users u INNER JOIN accounts a ON a.user_id = u.user_id", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ReturnUser u = GetUserForList(reader);
                        returnUsers.Add(u);
                    }

                }
            }
            return returnUsers;
        }

        /// <summary>
        /// Creates a new user with the provided details and sets their account balance to $1000
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User AddUser(string username, string password)
        {
            IPasswordHasher passwordHasher = new PasswordHasher();
            PasswordHash hash = passwordHasher.ComputeHash(password);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO users (username, password_hash, salt) VALUES (@username, @password_hash, @salt)", conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password_hash", hash.Password);
                cmd.Parameters.AddWithValue("@salt", hash.Salt);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT @@IDENTITY", conn);
                int userId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("INSERT INTO accounts (user_id, balance) VALUES (@userid, @startBalance)", conn);
                cmd.Parameters.AddWithValue("@userid", userId);
                cmd.Parameters.AddWithValue("@startBalance", startingBalance);
                cmd.ExecuteNonQuery();
            }

            return GetUser(username);
        }
        /// <summary>
        /// Sets the user details with data pulled from the database
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private User GetUserFromReader(SqlDataReader reader)
        {
            return new User()
            {
                UserId = Convert.ToInt32(reader["user_id"]),
                Username = Convert.ToString(reader["username"]),
                PasswordHash = Convert.ToString(reader["password_hash"]),
                Salt = Convert.ToString(reader["salt"])
            };
        }
        /// <summary>
        /// Gets the user details that are safe for storing in a list
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private ReturnUser GetUserForList(SqlDataReader reader)
        {
            return new ReturnUser()
            {
                UserId = Convert.ToInt32(reader["user_id"]),
                AccountId = Convert.ToInt32(reader["account_id"]),
                Username = Convert.ToString(reader["username"])
            };
        }
    }
}
