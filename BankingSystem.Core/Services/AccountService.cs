using BankingSystem.Core.Data;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankingDbContext _context;
        private readonly ILoggerService _logger;

        public AccountService(BankingDbContext context, ILoggerService logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Deposit(int accountId, decimal amount)
        {
            var account = _context.Accounts.Find(accountId);

            if (account == null)
                throw new Exception("Account not found");

            if (amount <= 0)
                throw new Exception("Invalid amount");

            account.Balance += amount;

            account.Transactions.Add(new Transaction
            {
                Amount = amount,
                Type = TransactionType.Deposit,
                Description = "Deposit"
            });

            _context.SaveChanges();

            _logger.Log($"DEPOSIT | Account:{accountId} | Amount:{amount}");
        }

        public void Withdraw(int accountId, decimal amount)
        {
            var account = _context.Accounts.Find(accountId);

            if (account == null)
                throw new Exception("Account not found");

            if (amount <= 0)
                throw new Exception("Invalid amount");

            if (account.Balance < amount)
                throw new Exception("Insufficient balance");

            account.Balance -= amount;

            account.Transactions.Add(new Transaction
            {
                Amount = amount,
                Type = TransactionType.Withdraw,
                Description = "Withdraw"
            });

            _context.SaveChanges();

            _logger.Log($"WITHDRAW | Account:{accountId} | Amount:{amount}");
        }

        public List<Account> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }
    }
}
