using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;

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
        public ActionResult GetPastTransfers(int userId)
        {
            return Ok(transfer.GetTransfers(userId));
        }
    }
}
