using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BankingSystem.Core.Data;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
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
                throw new Exception("Customer not found");

            var existingCard = _context.CreditCards
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (existingCard != null)
                throw new Exception("Customer already has a credit card");

            if (!IsValidLimit(cashLimit))
                throw new Exception("Invalid cash limit");

            var card = new CreditCard
            {
                CustomerId = customerId,
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
            var card = _context.CreditCards
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (card == null)
                throw new Exception("Credit card not found");

            if (!IsValidLimit(newLimit))
                throw new Exception("Invalid cash limit");

            card.CashLimit = newLimit;

            _context.SaveChanges();

            _logger.Log($"UPDATE_CREDIT_CARD_LIMIT | Customer:{customerId} | NewLimit:{newLimit}");
        }
        public CreditCard GetByCustomer(int customerId)
        {
            return _context.CreditCards
                .FirstOrDefault(c => c.CustomerId == customerId);
        }
        private bool IsValidLimit(decimal limit)
        {
            return limit >= 50000 && limit <= 250000;
        }
    }
}