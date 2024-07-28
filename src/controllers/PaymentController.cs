using Microsoft.AspNetCore.Mvc;
using MiniPaymentApi.Application.Services;
using MiniPaymentApi.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniPaymentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] PayRequest request)
        {
            var transaction = await _paymentService.PayAsync(request.BankId, request.Amount, request.OrderReference);
            return Ok(transaction);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelRequest request)
        {
            var result = await _paymentService.CancelAsync(request.TransactionId);
            if (!result)
            {
                return BadRequest("Transaction could not be cancelled.");
            }
            return Ok("Transaction cancelled successfully.");
        }

        [HttpPost("refund")]
        public async Task<IActionResult> Refund([FromBody] RefundRequest request)
        {
            var result = await _paymentService.RefundAsync(request.TransactionId);
            if (!result)
            {
                return BadRequest("Transaction could not be refunded.");
            }
            return Ok("Transaction refunded successfully.");
        }

        [HttpGet("report")]
        public async Task<IActionResult> Report([FromQuery] ReportRequest request)
        {
            var transactions = await _paymentService.GetReportAsync(request.BankId, request.Status, request.OrderReference, request.StartDate, request.EndDate);
            return Ok(transactions);
        }
    }

    public class PayRequest
    {
        public int BankId { get; set; }
        public decimal Amount { get; set; }
        public string OrderReference { get; set; }
    }

    public class CancelRequest
    {
        public Guid TransactionId { get; set; }
    }

    public class RefundRequest
    {
        public Guid TransactionId { get; set; }
    }

    public class ReportRequest
    {
        public int? BankId { get; set; }
        public string Status { get; set; }
        public string OrderReference { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
     }
}