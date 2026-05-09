using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Models
{
    public class CreditCard
    {
        public int Id { get; set; }

        public decimal CashLimit { get; set; }

        public DateTime ExpiryDate { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
