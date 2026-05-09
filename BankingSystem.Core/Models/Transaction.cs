using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Enums;

namespace BankingSystem.Core.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public TransactionType Type { get; set; }

        public string Description { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
