using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferType { get; set; } //1000 = Request 1001 = Send
        public string TransferDirection { get; set; } // "From" or "To" relative to other user
        public int TransferStatus { get; set; } = 2000; //2000 = Pending 2001 = Approved 2002 = Rejected
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public string OtherUser { get; set; }
        public int OtherUserId { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return string.Format("{0,-10}{1,5}{2,-25}{3,-10}{4}",
                TransferId, TransferDirection + ":", OtherUser, Amount.ToString("C"), SetTransferStatusString(TransferStatus));
        }

        /// <summary>
        /// Displays string details of provided TransferType int 
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Displays status string of provided TransferStatus int
        /// </summary>
        /// <param name="statusInt"></param>
        /// <returns></returns>
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
