using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

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

        public Balance GetBalance(int userId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT balance, user_id, account_id FROM accounts WHERE user_id = @user_id", conn);
                command.Parameters.AddWithValue("@user_id", userId);

                SqlDataReader reader = command.ExecuteReader();
                Balance balance = new Balance();
                while (reader.Read())
                {

                    balance.AccountBalance = Convert.ToDecimal(reader["balance"]);
                    balance.UserID = Convert.ToInt32(reader["user_id"]);
                    balance.AccountID = Convert.ToInt32(reader["account_id"]);
                }
                return balance;
            }
        }
    }
}
