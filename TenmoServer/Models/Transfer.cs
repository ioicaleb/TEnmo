using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferType { get; set; } //1000 = Request 1001 = Send
        public string TransferDirection { get; set; } //"From" or "To" always in relation to the account that is not the current user's
        public int TransferStatus { get; set; } // 2000 = Pending 2001 = Approved 2002 = Rejected
        public int AccountFrom { get; set; }
        public int AccountTo { get; set;  }
        public string OtherUser { get; set; } //Username of the account involved in the transfer that is not the current user's
        public int OtherUserId { get; set; }
        public decimal Amount { get; set; }
    }
}
