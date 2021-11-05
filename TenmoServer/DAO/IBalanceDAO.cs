using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
   public interface IBalanceDAO
    {
        Balance GetBalance(int userId);

        Balance UpdateBalance(Transfer transfer, int userId);
    }
}
