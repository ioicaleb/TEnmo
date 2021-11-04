using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public string TransferType { get; set; }
        public string TransferStatus { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set;  }
        public string OtherUser { get; set; }
        public decimal Amount { get; set; }
    }
}
