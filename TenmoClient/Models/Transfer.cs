using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public string TransferType { get; set; }
        public string TransferStatus { get; set; }
        public int AccountFromId { get; set; }
        public int AccountToId { get; set; }
        public string OtherUser { get; set; }
        public decimal Amount { get; set; }
        
        public override string ToString()
        {

            return string.Format("{0,-15}{1,-5}{2,-25}{3}",
                TransferId, TransferType + ":", OtherUser, Amount.ToString("C"));
        }
    }

}
