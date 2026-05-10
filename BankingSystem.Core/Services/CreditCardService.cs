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
                throw new CustomerNotFoundException();

            var existingCard = _context.CreditCards.Find(customerId);

            if (existingCard != null)
                throw new InvalidOperationBusinessException("Customer already has a credit card");

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

            _logger.Log($"CREATE_CREDIT_CARD | Customer:{customerId} | Limit:{cashLimit}");

            return card;
        }

        public void UpdateLimit(int customerId, decimal newLimit)
        {
            var card = _context.CreditCards.Find(customerId);

            if (card == null)
                throw new CreditCardNotFoundException();

            ValidateLimit(newLimit);

            card.CashLimit = newLimit;

            _context.SaveChanges();

            _logger.Log($"UPDATE_CREDIT_CARD_LIMIT | Customer:{customerId} | NewLimit:{newLimit}");
        }

        public CreditCard GetByCustomer(int customerId)
        {
            return _context.CreditCards.Find(customerId);
        }

        private void ValidateLimit(decimal limit)
        {
            if (limit < 50000 || limit > 250000)
                throw new InvalidOperationBusinessException("Invalid cash limit (50k - 250k)");
        }
    }
}