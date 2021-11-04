﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [HttpGet]
        public ActionResult GetBalance(int userId)
        {

            bool permittedUser = VerifyUser(userId);

            if (!permittedUser)

            {
                return Forbid();
            }

            Balance newBalance = balance.GetBalance(userId);

            return Ok(newBalance);
        }

        [HttpPut]
        public ActionResult UpdateBalance(Transfer transfer, int userId)
        {
            bool permittedUser = VerifyUser(userId);
            if (!permittedUser)
            {
                return Forbid();
            }

            return Ok(transfer);
        }

        private bool VerifyUser(int userId)
        {
            string userSub = User.FindFirst("sub").Value;
            int tokenUserId = int.Parse(userSub);

            if (tokenUserId != userId)
            {
                return false;
            }

            return true;
        }
    }
}
