using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferType { get; set; }
        public string TransferDirection { get; set; }
        public int TransferStatus { get; set; } = 2000;
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public string OtherUser { get; set; }
        public int OtherUserId { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return string.Format("{0,-15}{1,-5}{2,-25}{3,-10}{4}",
                TransferId, TransferDirection + ":", OtherUser, Amount.ToString("C"), SetTransferStatusString(TransferStatus));
        }


        public string SetTransferTypeString(int typeId)
        {
            switch (typeId)
            {
                case 1000:
                    return "Request";
                case 1001:
                    return "Send";
                default:
                    return null;
            }
        }

        public string SetTransferStatusString(int statusInt)
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
