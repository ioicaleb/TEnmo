using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Models;

namespace TenmoClient
{
    public class RequestHandler
    {
        public Transfer ManagePendingRequest(bool approved, Transfer transfer)
        {
            if (approved)
            {
                transfer.TransferStatus = 2001;
            }
            else
            {
                transfer.TransferStatus = 2002;
            }
            return transfer;
        }
    }
}
