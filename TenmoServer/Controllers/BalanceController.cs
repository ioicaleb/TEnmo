using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// Checks the balance of the logged in user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBalance(int userId)
        {

            bool permittedUser = VerifyUser(userId);//Checks that the ID of the account being accessed is the same as the users ID

            if (!permittedUser)

            {
                return Forbid();
            }

            Balance newBalance = balance.GetBalance(userId);

            return Ok(newBalance);
        }
        /// <summary>
        /// Completes the transaction by changing the balances of both users involved in the transfer
        /// Verifies that the transaction will not overdraft the account
        /// </summary>
        /// <param name="transfer"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public ActionResult UpdateBalance([FromBody] Transfer transfer, int userId)
        {
            bool permittedUser = VerifyUser(userId);
            if (!permittedUser)
            {
                return Forbid();
            }

            decimal newBalance = balance.UpdateBalance(transfer, userId);

            if (newBalance == -1)//If transaction would overdraft the account, the method returns -1 instead of completing the transfer
            {
                return BadRequest("Transfer would overdraft account");
            }

            return Ok(newBalance);
        }

        /// <summary>
        /// Checks that the user ID accessing the account is the same as the ID of the account being accessed
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool VerifyUser(int userId)
        {
            string userSub = User.FindFirst("sub").Value; //Checks the user token for the "sub" value which is the token's interpretation of the UserID
            int tokenUserId = int.Parse(userSub);

            if (tokenUserId != userId)
            {
                return false;
            }

            return true;
        }
    }
}
