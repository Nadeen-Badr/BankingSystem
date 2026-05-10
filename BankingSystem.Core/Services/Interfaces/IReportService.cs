using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BankingSystem.Core.DTOs;

namespace BankingSystem.Core.Services.Interfaces
{
    public interface IReportService
    {
        // BANK STATISTICS
        BankStatisticsDto GetBankStatistics();

        // CUSTOMER REPORT
        CustomerReportDto GetCustomerReport(int customerId);
    }
}