using BankingSystem.Core.Data;
using BankingSystem.Core.DTOs;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BankingSystem.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly BankingDbContext _context;

        public ReportService(BankingDbContext context)
        {
            _context = context;
        }

        // ================= BANK STATISTICS =================
        public BankStatisticsDto GetBankStatistics()
        {
            return new BankStatisticsDto
            {
                TotalCustomers = _context.Customers.Count(),

                TotalAccounts = _context.Accounts.Count(),

                TotalAssets = _context.Accounts
                    .Select(a => (decimal?)a.Balance)
                    .Sum() ?? 0,

                TotalCertificates = _context.Certificates.Count(),

                TotalCreditCards = _context.CreditCards.Count()
            };
        }

         //================= CUSTOMER REPORT =================
        public CustomerReportDto GetCustomerReport(int customerId)
        {
            var customer = _context.Customers
                .FirstOrDefault(c => c.Id == customerId);

            if (customer == null)
                return null;

            var accounts = _context.Accounts
                .Include("Transactions")
                .Where(a => a.CustomerId == customerId)
                .ToList();

            var accountIds = accounts.Select(a => a.Id).ToList();

            var certificates = _context.Certificates
                .Where(c => c.CustomerId == customerId)
                .ToList();

            var creditCards = _context.CreditCards
                .Where(c => c.Id == customerId)
                .ToList();

            var transactions = _context.Transactions
                .Where(t => accountIds.Contains(t.AccountId))
                .ToList();

            return new CustomerReportDto
            {
                CustomerName = customer.Name,
                Accounts = accounts,
                Transactions = transactions,
                Certificates = certificates,
                CreditCards = creditCards,
                TotalBalance = accounts.Sum(a => a.Balance)
            };
        }

    }
}