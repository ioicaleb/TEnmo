using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferDAO : ITransferDAO
    {
        private readonly string connStr;

        private readonly string SqlGetTransfers =
            "SELECT t.transfer_id, t.transfer_type_id, t.transfer_status_id, t.account_from, t.account_to, t.amount, a.account_id AS user_account_id " +
            "FROM transfers t " +
            "INNER JOIN accounts a ON (a.account_id = t.account_from OR a.account_id = t.account_to) " +
            "WHERE a.user_id = @user_id " +
            "AND (@transfer_id = 0 OR t.transfer_id = @transfer_id)";

        private readonly string SqlCreateTransfer =
            "INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from,account_to,amount) " +
            "VALUES(@transfer_type_id , @transfer_status_id, (SELECT account_id FROM accounts WHERE user_id = @account_from),(SELECT account_id FROM accounts WHERE user_id = @account_to), @amount) " +
            "SELECT @@IDENTITY ";

        private readonly string SqlUpdateTransfer =
            "UPDATE transfers SET transfer_type_id = @transfer_type_id, transfer_status_id = @transfer_status_id, account_from = @account_from, " +
            "account_to = @account_to, amount = @amount WHERE transfer_id = @transfer_id " +
            "SELECT transfer_id FROM transfers WHERE transfer_id = @transfer_id";


        public TransferDAO(string connStr)
        {
            if (string.IsNullOrWhiteSpace(connStr))
            {
                throw new ArgumentNullException(nameof(connStr));
            }
            this.connStr = connStr;
        }

        public List<Transfer> GetTransfers(int userId, int transferId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(SqlGetTransfers, conn))
                {
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@transfer_id", transferId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Transfer> transfers = new List<Transfer>();
                        while (reader.Read())
                        {
                            int userAccountId = Convert.ToInt32(reader["user_account_id"]);

                            Transfer transfer = new Transfer
                            {
                                TransferId = Convert.ToInt32(reader["transfer_id"]),
                                TransferStatus = Convert.ToInt32(reader["transfer_status_id"]),
                                Amount = Convert.ToDecimal(reader["amount"]),
                                AccountFrom = Convert.ToInt32(reader["account_from"]),
                                AccountTo = Convert.ToInt32(reader["account_to"]),
                                TransferType = Convert.ToInt32(reader["transfer_type_id"])
                            };
                            transfer.TransferDirection = SetTransferDirection(transfer.AccountTo, userAccountId);
                            transfers.Add(transfer);
                        }
                        return transfers;
                    }
                }
            }
        }
        public Transfer CreateNewTransfer(Transfer transfer, int userId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(SqlCreateTransfer, conn))
                {
                    int fromAccount = 0;
                    int toAccount = 0;
                    if (transfer.TransferType == 1001)
                    {
                        fromAccount = userId;
                        toAccount = transfer.OtherUserId;
                    }
                    else
                    {
                        toAccount = userId;
                        fromAccount = transfer.OtherUserId;
                    }

                    command.Parameters.AddWithValue("@transfer_type_id", transfer.TransferType);
                    command.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatus);
                    command.Parameters.AddWithValue("@account_from", fromAccount);
                    command.Parameters.AddWithValue("@account_to", toAccount);
                    command.Parameters.AddWithValue("@amount", transfer.Amount);
                    transfer.TransferId = Convert.ToInt32(command.ExecuteScalar());
                    return transfer;
                }
            }
        }

        public bool UpdateTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(SqlUpdateTransfer, conn))
                {
                    cmd.Parameters.AddWithValue("@transfer_id", transfer.TransferId);
                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferType);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatus);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                    string transferString = Convert.ToString(cmd.ExecuteScalar());

                    if (!int.TryParse(transferString, out int transferId))
                    {
                        return false;
                    }
                    return true;
                }

            }
        }

        public string SetTransferDirection(int accountToId, int userAccountId)
        {
            if (accountToId == userAccountId)
            {
                return "From";
            }
            else
            {
                return "To";
            }
        }
    }
}
