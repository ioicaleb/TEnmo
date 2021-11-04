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

        private readonly string SqlSendTransfer = "UPDATE accounts SET balance = balance - @amount WHERE user_id = @user_id";

        private readonly string SqlReceiveTransfer = "UPDATE accounts SET balance = balance + @amount WHERE user_id = @user_id";
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
                using (SqlCommand command = new SqlCommand("SELECT balance, user_id, account_id FROM accounts WHERE user_id = @user_id", conn))
                {
                    command.Parameters.AddWithValue("@user_id", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        { 
                        Balance balance = new Balance
                        {
                            UserID = Convert.ToInt32(reader["user_id"]),
                            AccountBalance = Convert.ToDecimal(reader["balance"]),
                            AccountID = Convert.ToInt32(reader["account_id"])
                        };
                        return balance;
                        }

                        return null;
                    }
                }
            }
        }
    }
}
