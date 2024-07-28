using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniPaymentApi.Domain.Entities;
using MiniPaymentApi.Infrastructure.Data;

namespace MiniPaymentApi.Application.Services
{
    public class PaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> PayAsync(int bankId, decimal amount, string orderReference)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                BankId = bankId,
                TotalAmount = amount,
                NetAmount = amount,
                Status = "Success",
                OrderReference = orderReference,
                TransactionDate = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            var transactionDetail = new TransactionDetail
            {
                Id = Guid.NewGuid(),
                TransactionId = transaction.Id,
                TransactionType = "Sale",
                Status = "Success",
                Amount = amount
            };

            _context.TransactionDetails.Add(transactionDetail);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<bool> CancelAsync(Guid transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null || transaction.TransactionDate.Date != DateTime.UtcNow.Date)
            {
                return false;
            }

            transaction.NetAmount -= transaction.TotalAmount;
            transaction.Status = "Cancelled";

            var transactionDetail = new TransactionDetail
            {
                Id = Guid.NewGuid(),
                TransactionId = transaction.Id,
                TransactionType = "Cancel",
                Status = "Success",
                Amount = transaction.TotalAmount
            };

            _context.TransactionDetails.Add(transactionDetail);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RefundAsync(Guid transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            if (transaction == null || (DateTime.UtcNow - transaction.TransactionDate).Days < 1)
            {
                return false;
            }

            transaction.NetAmount -= transaction.TotalAmount;
            transaction.Status = "Refunded";

            var transactionDetail = new TransactionDetail
            {
                Id = Guid.NewGuid(),
                TransactionId = transaction.Id,
                TransactionType = "Refund",
                Status = "Success",
                Amount = transaction.TotalAmount
            };

            _context.TransactionDetails.Add(transactionDetail);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IQueryable<Transaction>> GetReportAsync(int? bankId, string status, string orderReference, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Transactions.AsQueryable();

            if (bankId.HasValue)
            {
                query = query.Where(t => t.BankId == bankId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(t => t.Status == status);
            }

            if (!string.IsNullOrEmpty(orderReference))
            {
                query = query.Where(t => t.OrderReference == orderReference);
            }

            if (startDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate <= endDate.Value);
            }

            return query;
        }
    }
}
