using System;
using System.Collections.Generic;
using System.Linq;
using BankingSystem.Core.Data;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;

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
            // 1. Find the customer
            var customer = _context.Customers.Find(customerId);

            if (customer == null)
                throw new Exception("Customer not found");

            // 2. Check existence using Id (since it's a shared PK)
            var existingCard = _context.CreditCards.Find(customerId);

            if (existingCard != null)
                throw new Exception("Customer already has a credit card");

            // 3. Business Logic Validation
            if (!IsValidLimit(cashLimit))
                throw new Exception("Invalid cash limit");

            // 4. Create the card
            var card = new CreditCard
            {
                Id = customerId, // Primary Key is the CustomerId
                Customer = customer, // Navigation property link
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
            // Search by Id because CreditCard.Id == CustomerId
            var card = _context.CreditCards.Find(customerId);

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
            // Direct lookup by ID is faster than FirstOrDefault
            return _context.CreditCards.Find(customerId);
        }

        private bool IsValidLimit(decimal limit)
        {
            return limit >= 50000 && limit <= 250000;
        }
    }
}