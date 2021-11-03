using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Route("/balance")]
    [Authorize]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceDAO balance;
        public BalanceController(IBalanceDAO balance)
        {
            this.balance = balance;
        }

        [HttpGet("{userId}")]
        public ActionResult GetBalance(int userId)
        {
            return Ok(this.balance.GetBalance(userId));
        }
    }
}
