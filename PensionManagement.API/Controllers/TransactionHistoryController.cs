using Microsoft.AspNetCore.Mvc;
using PensionManagement.Application.Contracts;
using PensionManagement.Application.DTOs;

namespace PensionManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionHistoryController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;

        public TransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            _transactionHistoryService = transactionHistoryService;
        }

        public ActionResult<IEnumerable<TransactionHistoryDto>> GetTransactionHistory()
        {
            var transactionHistory = _transactionHistoryService.GetTransactionHistory;
            return Ok (transactionHistory);
        }
    }
}
