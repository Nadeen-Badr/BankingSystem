using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Enums;

namespace BankingSystem.Core.Models
{
        public class Certificate
        {
            public int Id { get; set; }

            public decimal Price { get; set; }

            public CertificatePeriod Period { get; set; }

            public decimal InterestRate { get; set; }

            public DateTime StartDate { get; set; } = DateTime.Now;

            public int CustomerId { get; set; }
            public Customer Customer { get; set; }
        }
   
}
