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
   public class TransferController : ControllerBase
    {
        private readonly ITransferDAO transfer;

        public TransferController(ITransferDAO transfer)
        {
            this.transfer = transfer;
        }

        [HttpGet("{userId}")]
        public ActionResult GetTransfers(int userId, int transferId)
        {
            List<Transfer> transfers = transfer.GetTransfers(userId, transferId);
            if (transfers.Count < 1)
            {
                return NotFound();
            }
            return Ok(transfers);
        }
    }
}
