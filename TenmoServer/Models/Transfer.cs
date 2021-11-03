using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferType { get; set; }
        public int TransferStatus { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set;  }
        public decimal Amount { get; set; }
    }
}
