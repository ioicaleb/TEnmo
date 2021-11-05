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

        [HttpPost("{userId}")]
        public ActionResult CreateTransfer(Transfer newTransfer, int userId)
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
                if (newTransfer.TransferType == 1001 && newTransfer.TransferType != 2002)
                {
                    newTransfer.TransferStatus = 2001;
                    transfer.UpdateTransfer(newTransfer);
                }
                string location = $"/transfer/{userId}?transferId={newTransfer.TransferId}";
                return Created(location, newTransfer);
            }
            return BadRequest();
        }

        [HttpPut]
        public ActionResult UpdateTransferDetails(Transfer updatedTransfer)
        {
            if (transfer.UpdateTransfer(updatedTransfer))
            {
                return Ok(updatedTransfer);
            }
            return BadRequest("Could not update transfer details");
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
