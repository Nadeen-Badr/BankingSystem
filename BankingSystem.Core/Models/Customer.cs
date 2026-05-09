using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Enums;
using System.Collections.Generic;

namespace BankingSystem.Core.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }

        // Navigation Properties
        public virtual List<Account> Accounts { get; set; }
        public virtual List<Certificate> Certificates { get; set; }
        public CreditCard CreditCard { get; set; }
    }
}
