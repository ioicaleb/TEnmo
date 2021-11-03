using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoServer.Models
{
    public class Balance
    {
        public int AccountID { get; set; }

        public int UserID { get; set; }

        public decimal AccountBalance { get; set; }
    }
}
