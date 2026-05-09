using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Enums;

namespace BankingSystem.Core.Models
{
    // im making this class abstract because Because there is no generic “Account” Only: SavingAccount or SalaryAccount
    public abstract class Account
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public AccountType AccountType { get; set; }
        public int CustomerId { get; set; }
        public virtual List<Transaction> Transactions { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
