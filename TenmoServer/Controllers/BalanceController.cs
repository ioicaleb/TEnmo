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
            Balance newBalance = balance.GetBalance(userId);

            if (newBalance.UserID != userId)
            {
                return Forbid();
            }
            return Ok(newBalance);
        }

        [HttpPut]
        public ActionResult UpdateBalance(Transfer transfer)
        {
            return Ok(transfer);
        }
    }
}
