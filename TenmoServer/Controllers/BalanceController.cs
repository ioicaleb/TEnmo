using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("/[controller]")]
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
            Balance accountBalance = balance.GetBalance(userId);

            if (accountBalance.UserID != userId)
            {
                return Forbid();
            }
            return Ok();
        }
    }
}
