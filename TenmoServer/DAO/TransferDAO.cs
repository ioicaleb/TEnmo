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
            "SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to, amount " +
            "FROM transfers WHERE account_from = @account_id OR account_to = @account_id";

        public TransferDAO(string connStr)
        {
            if (string.IsNullOrWhiteSpace(connStr))
            {
                throw new ArgumentNullException(nameof(connStr));
            }
            this.connStr = connStr;
        }

        public List<Transfer> GetTransfers(int accountId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(SqlGetTransfers, conn))
                {
                    cmd.Parameters.AddWithValue("@account_id", accountId);
                    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Transfer> transfers = new List<Transfer>();
                        while (reader.Read())
                        {
                            int typeInt = Convert.ToInt32(reader["transfer_type_id"]);
                            int statusInt = Convert.ToInt32(reader["transfer_status_id"]);

                            Transfer transfer = new Transfer
                            {
                                TransferId = Convert.ToInt32(reader["transfer_id"]),
                                TransferType = ConvertTypeIntToString(typeInt),
                                TransferStatus = ConvertStatusIntToString(statusInt),
                                AccountFrom = Convert.ToInt32(reader["account_from"]),
                                AccountTo = Convert.ToInt32(reader["account_to"]),
                                Amount = Convert.ToDecimal(reader["amount"])
                            };
                            transfers.Add(transfer);
                        }
                        return transfers;
                    }
                }
            }
        }

        public string ConvertTypeIntToString(int typeInt)
        {
            switch (typeInt)
            {
                case 1000:
                    return "Request";
                case 1001:
                    return "Send";
                default:
                    return null;
            }
        }

        public string ConvertStatusIntToString(int statusInt)
        {
            switch (statusInt)
            {
                case 2000:
                    return "Pending";
                case 2001:
                    return "Approved";
                case 2002:
                    return "Rejected";
                default:
                    return null;
            }
        }
    }
}
