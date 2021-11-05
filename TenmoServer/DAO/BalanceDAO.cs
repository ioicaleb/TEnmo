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
            "UPDATE accounts SET balance = balance - @amount WHERE account_id = @account_from " +
            "UPDATE accounts SET balance = balance + @amount WHERE account_id = @account_to " +
            "SELECT balance FROM accounts WHERE user_id = @user_id";

        private readonly string SqlCheckBalances =
            "SELECT balance FROM accounts a INNER JOIN transfers t ON a.account_id = t.account_from WHERE account_id = @account_id ";

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

        public decimal UpdateBalance(Transfer transfer, int userId)
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

                    if (CheckTransferBalance(transfer, conn))
                    {
                        return Convert.ToDecimal(command.ExecuteScalar());
                    }

                    return -1;
                }
            }
        }

        public bool CheckTransferBalance(Transfer transfer, SqlConnection conn)
        {
            using (SqlCommand comm = new SqlCommand(SqlCheckBalances, conn))
            {
                comm.Parameters.AddWithValue("@account_id", transfer.AccountFrom);

                decimal balance = Convert.ToDecimal(comm.ExecuteScalar());
                if (balance - transfer.Amount < 0)
                {
                    return false;
                }

            }
            return true;
        }
    }
}
