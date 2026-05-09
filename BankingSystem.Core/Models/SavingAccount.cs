using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Models
{
   
        public class SavingAccount : Account
        {
            public decimal InterestRate { get; set; } = 0.05m;
        }
    
}
