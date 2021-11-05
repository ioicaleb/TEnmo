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
                int approvedInt = ConsoleService.GetInteger("1: Approve\n2: Reject\n3: Do Nothing\n");
                switch (approvedInt)
                {
                    case 1:
                        transfer.TransferStatus = 2001;
                        break;
                    case 2:
                        transfer.TransferStatus = 2002;
                        break;
                    default:
                        break;

                }
            }
            else
            {
                int approvedInt = ConsoleService.GetInteger("1: Cancel Request\n2: Do Nothing\n");
                switch (approvedInt)
                {
                    case 1:
                        transfer.TransferStatus = 2002;
                        break;
                    default:
                        break;

                }
            }
            return transfer;
        }
    }
}
