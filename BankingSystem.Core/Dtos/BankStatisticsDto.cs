using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.DTOs
{
    public class BankStatisticsDto
    {
        public int TotalCustomers { get; set; }

        public int TotalAccounts { get; set; }

        public decimal TotalAssets { get; set; }

        public int TotalCertificates { get; set; }

        public int TotalCreditCards { get; set; }
    }
}