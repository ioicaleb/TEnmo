using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public string TransferType { get; set; }
        public string TransferStatus { get; set; } = "Pending";
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public string OtherUser { get; set; }
        public decimal Amount { get; set; }
        
        public override string ToString()
        {
            return string.Format("{0,-15}{1,-5}{2,-25}{3,-10}{4}",
                TransferId, TransferType + ":", OtherUser, Amount.ToString("C"), TransferStatus);
        }
    }

}
