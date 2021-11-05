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

<<<<<<< HEAD
        private readonly string SqlUpdateBalances = 
            "UPDATE accounts SET balance = balance - @amount WHERE account_id = @fromaccount_id "+
            "UPDATE accounts SET balance = balance + @amount WHERE account_id = @toaccount_id "+
=======
        private readonly string SqlUpdateBalances =
            "UPDATE accounts SET balance = balance - @amount WHERE account_id = @account_from " +
            "UPDATE accounts SET balance = balance + @amount WHERE account_id = @account_to " +
>>>>>>> 6b5c8ed27fe7493277e4ae4ead15b9e3c5734b1d
            "SELECT balance FROM accounts WHERE user_id = @user_id";
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

        public Balance UpdateBalance(Transfer transfer, int userId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(SqlUpdateBalances, conn))
                {
                    command.Parameters.AddWithValue("@amount", transfer.Amount);
                    command.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    command.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    command.Parameters.AddWithValue("@user_id", userId);

                    Balance balance = new Balance
                    {
<<<<<<< HEAD
                        toAccount = userId;
                        fromAccount = transfer.OtherUserId;
                    }
                        command.Parameters.AddWithValue("@amount", transfer.Amount);
                        command.Parameters.AddWithValue("@fromaccount_id", fromAccount);
                        command.Parameters.AddWithValue("@toaccount_id", toAccount);
                        command.Parameters.AddWithValue("@user_id", userId);
                    
                    command.ExecuteNonQuery();
                    return true;
=======
                        AccountBalance = Convert.ToDecimal(command.ExecuteScalar())
                    };
                    return balance;
>>>>>>> 6b5c8ed27fe7493277e4ae4ead15b9e3c5734b1d
                }
            }
        }

    }
}
