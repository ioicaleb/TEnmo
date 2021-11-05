using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;

namespace TenmoClient
{
    public class RequestHandler
    {
        public Transfer ManagePendingRequest(Transfer transfer)
        {
            if (transfer.TransferType == 1000 && transfer.TransferDirection == "To")
            {
                bool approved = ConsoleService.GetApproveBool("Do you want to approve (A) or reject (r) the request?: ");
                if (approved)
                {
                    transfer.TransferStatus = 2001;
                }
                else
                {
                    transfer.TransferStatus = 2002;
                }
            }
            return transfer;
        }
    }
}
