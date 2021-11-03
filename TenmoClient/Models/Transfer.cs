using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferId { get; }
        public int TransferType { get; }
        public int TransferStatus { get; set; }
        public int AccountFrom { get; }
        public int AccountTo { get; }
        public decimal Amount { get; set; }
    }
}
