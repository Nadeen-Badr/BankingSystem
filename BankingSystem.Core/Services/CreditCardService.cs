using BankingSystem.Core.Data;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.Core.Exceptions;
using System;

namespace BankingSystem.Core.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly BankingDbContext _context;
        private readonly ILoggerService _logger;

        public CreditCardService(BankingDbContext context, ILoggerService logger)
        {
            _context = context;
            _logger = logger;
        }

        public CreditCard CreateCard(int customerId, decimal cashLimit)
        {
            var customer = _context.Customers.Find(customerId);

            if (customer == null)
            {
                _logger.Log($"CREATE_CREDIT_CARD_FAILED | Customer:{customerId} | Reason: Customer not found");
                throw new CustomerNotFoundException();
            }

            var existingCard = _context.CreditCards.Find(customerId);

            if (existingCard != null)
            {
                _logger.Log($"CREATE_CREDIT_CARD_FAILED | Customer:{customerId} | Reason: Already has card");
                throw new InvalidOperationBusinessException("Customer already has a credit card");
            }

            ValidateLimit(cashLimit);

            var card = new CreditCard
            {
                Id = customerId,
                Customer = customer,
                CashLimit = cashLimit,
                ExpiryDate = DateTime.Now.AddYears(10)
            };

            _context.CreditCards.Add(card);
            _context.SaveChanges();

            _logger.Log($"CREATE_CREDIT_CARD_SUCCESS | Customer:{customerId} | Limit:{cashLimit} | Expiry:{card.ExpiryDate:yyyy-MM-dd}");

            return card;
        }


        public void UpdateLimit(int customerId, decimal newLimit)
        {
            var card = _context.CreditCards.Find(customerId);

            if (card == null)
            {
                _logger.Log($"UPDATE_CREDIT_CARD_FAILED | Customer:{customerId} | Reason: Card not found");
                throw new CreditCardNotFoundException();
            }

            ValidateLimit(newLimit);

            var oldLimit = card.CashLimit;
            card.CashLimit = newLimit;

            _context.SaveChanges();

            _logger.Log($"UPDATE_CREDIT_CARD_SUCCESS | Customer:{customerId} | OldLimit:{oldLimit} | NewLimit:{newLimit}");
        }


        public CreditCard GetByCustomer(int customerId)
        {
            return _context.CreditCards.Find(customerId);
        }

        private void ValidateLimit(decimal limit)
        {
            if (limit < 50000 || limit > 250000)
            {
                _logger.Log($"CREDIT_CARD_VALIDATION_FAILED | Limit:{limit} | Reason: Out of allowed range");
                throw new InvalidOperationBusinessException("Invalid cash limit (50k - 250k)");
            }
        }
    }
}