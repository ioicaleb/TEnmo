using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public class BalanceDAO : IBalanceDAO
    {
        private readonly string connStr;
        public BalanceDAO(string dbConnectionString)
        {
            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                throw new ArgumentNullException(nameof(dbConnectionString));
            }
            this.connStr = dbConnectionString;
        }

        public decimal GetBalance(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT balance FROM accounts WHERE user_id = @user_id", conn);
                command.Parameters.AddWithValue("@user_id", userId);

                SqlDataReader reader = command.ExecuteReader();
                decimal accountBalance = 0M;
                while (reader.Read())
                {

                     accountBalance = Convert.ToDecimal(reader["balance"]);
                    
                }
                return accountBalance;
            }
        }
    }
}
