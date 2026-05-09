using BankingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BankingSystem.Core.Data;
using System.Linq;

namespace BankingSystem.Core.Services
{
    public class ReportService
    {
        private readonly BankingDbContext _context;

        public ReportService(BankingDbContext context)
        {
            _context = context;
        }

        public int GetTotalCustomers()
        {
            return _context.Customers.Count();
        }

        public int GetTotalAccounts()
        {
            return _context.Accounts.Count();
        }

        public decimal GetTotalBalance()
        {
            return _context.Accounts
                .Select(a => (decimal?)a.Balance)
                .Sum() ?? 0;
        }

        public int GetTotalCertificates()
        {
            return _context.Certificates.Count();
        }

        public int GetTotalCreditCards()
        {
            return _context.CreditCards.Count();
        }
    }
}