using BankingSystem.Core.Data;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Exceptions;
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

        public AccountService()
        {
        }

        public void Deposit(int accountId, decimal amount)
        {
            var account = _context.Accounts.Find(accountId);

            if (account == null)
                throw new AccountNotFoundException();

            if (amount <= 0)
                throw new InvalidOperationBusinessException("Invalid deposit amount");

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
                throw new AccountNotFoundException();

            if (amount <= 0)
                throw new InvalidOperationBusinessException("Invalid withdraw amount");

            if (account.Balance < amount)
                throw new InvalidOperationBusinessException("Insufficient balance");

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
        public Account CreateSavingAccount(int customerId)
        {
            var customer = _context.Customers.Find(customerId);

            if (customer == null)
                throw new CustomerNotFoundException();

            var account = new SavingAccount
            {
                CustomerId = customerId,
                AccountType = AccountType.Saving,
                Balance = 0
            };

            _context.Accounts.Add(account);
            _context.SaveChanges();

            _logger.Log($"CREATE_SAVING_ACCOUNT | Customer:{customerId}");

            return account;
        }
        public Account CreateSalaryAccount(int customerId)
        {
            var customer = _context.Customers.Find(customerId);

            if (customer == null)
                throw new CustomerNotFoundException();

            var account = new SalaryAccount
            {
                CustomerId = customerId,
                AccountType = AccountType.Salary,
                Balance = 0
            };

            _context.Accounts.Add(account);
            _context.SaveChanges();

            _logger.Log($"CREATE_SALARY_ACCOUNT | Customer:{customerId}");

            return account;
        }
        public void CloseAccount(int accountId)
        {
            var account = _context.Accounts.Find(accountId);

            if (account == null)
                throw new AccountNotFoundException();

            _context.Accounts.Remove(account);
            _context.SaveChanges();

            _logger.Log($"CLOSE_ACCOUNT | Account:{accountId}");
        }
        public List<Account> GetAccountsByCustomer(int customerId)
        {
            return _context.Accounts
                .Include("Transactions")
                .Where(a => a.CustomerId == customerId)
                .ToList();
        }


        public void Withdraw(Account account, int v)
        {
            throw new NotImplementedException();
        }
    }
}
