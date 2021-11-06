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
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private readonly ITransferDAO transfer;

        public TransferController(ITransferDAO transfer)
        {
            this.transfer = transfer;
        }
        /// <summary>
        /// Gets a list of all transfers in which the current user is either the sender or recipient
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="transferId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTransfers(int userId, int transferId)
        {
            bool permittedUser = VerifyUser(userId);
            if (!permittedUser)
            {
                return Forbid();
            }
            List<Transfer> transfers = transfer.GetTransfers(userId, transferId);
            if (transfers.Count < 1)
            {
                return NotFound();
            }
            return Ok(transfers);
        }
        /// <summary>
        /// Creates a log of a pending transaction
        /// Approves the transaction if the sender is creating the transaction and the transaction has not already been rejected
        /// </summary>
        /// <param name="newTransfer"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("{userId}")]
        public ActionResult CreateTransfer([FromBody]Transfer newTransfer, int userId)
        {
            bool permittedUser = VerifyUser(userId);
            if (!permittedUser)
            {
                return Forbid();
            }
            newTransfer = transfer.CreateNewTransfer(newTransfer, userId);
            if (newTransfer != null)
            {
                newTransfer = transfer.GetTransfers(userId, newTransfer.TransferId)[0];
                if (newTransfer.TransferType == 1001 && newTransfer.TransferStatus != 2002)
                {
                    newTransfer.TransferStatus = 2001;
                    transfer.UpdateTransfer(newTransfer);
                }
                string location = $"/transfer/{userId}?transferId={newTransfer.TransferId}";
                return Created(location, newTransfer);
            }
            return BadRequest();
        }
        /// <summary>
        /// Updates the transfer status
        /// </summary>
        /// <param name="updatedTransfer"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public ActionResult UpdateTransferDetails(Transfer updatedTransfer, int userId)
        {
            bool permittedUser = VerifyUser(userId);
            if (!permittedUser)
            {
                return Forbid();
            }
            if (transfer.UpdateTransfer(updatedTransfer))
            {
                return Ok(updatedTransfer);
            }
            return BadRequest("Could not find transfer details");
        }
        /// <summary>
        /// Checks that the user creating the transaction is the current user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
