using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        public List<Transfer> GetTransfers(int accountId, int transferId);
        public Transfer CreateNewTransfer(Transfer transfer, int userId);
        public bool UpdateTransfer(Transfer transfer);
    }
}
