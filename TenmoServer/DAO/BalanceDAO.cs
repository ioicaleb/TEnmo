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

        private readonly string SqlUpdateBalances = 
            "UPDATE accounts SET balance = balance - @amount WHERE user_id = @fromaccount_id "+
            "UPDATE accounts SET balance = balance + @amount WHERE user_id = @toaccount_id";
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

        public bool UpdateBalance(Transfer transfer, int userId)
        {
            using(SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(SqlUpdateBalances, conn))
                {
                    int fromAccount = 0;
                    int toAccount = 0;
                    if(transfer.TransferType == 1001)
                    {
                        fromAccount = userId;
                        toAccount = transfer.OtherUserId;
                    }
                    else
                    {
                        toAccount = userId;
                        fromAccount = transfer.OtherUserId;
                    }
                        command.Parameters.AddWithValue("@amount", transfer.Amount);
                        command.Parameters.AddWithValue("@fromaccount_id", fromAccount);
                        command.Parameters.AddWithValue("@toaccount_id", toAccount);
                    
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }
        
    }
}
