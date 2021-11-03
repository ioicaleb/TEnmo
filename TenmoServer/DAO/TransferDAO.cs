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

        private readonly string SqlGetTransfers = "SELECT transfer_id, transfer_type_id, transfer_status_id, account_from, account_to FROM transfers WHERE account_from = @account_id OR account_to = @account_id";

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

                using (SqlCommand cmd = new SqlCommand(SqlGetTransfers))
                {
                    cmd.Parameters.AddWithValue("@account_id", accountId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Transfer> transfers = new List<Transfer>();
                        while (reader.Read())
                        {
                            Transfer transfer = new Transfer
                            {
                                TransferId = Convert.ToInt32(reader["transfer_id"])
                            };
                        }
                    }
                }
                    return new List<Transfer>();
            }
        }
    }
}
