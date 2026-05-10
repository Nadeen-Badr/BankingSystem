using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BankingSystem.Core.Models;
using System.Collections.Generic;

namespace BankingSystem.Core.DTOs
{
    public class CustomerReportDto
    {
        public string CustomerName { get; set; }

        public decimal TotalBalance { get; set; }

        public List<Account> Accounts { get; set; }

        public List<Transaction> Transactions { get; set; }

        public List<Certificate> Certificates { get; set; }

        public List<CreditCard> CreditCards { get; set; }
    }
}