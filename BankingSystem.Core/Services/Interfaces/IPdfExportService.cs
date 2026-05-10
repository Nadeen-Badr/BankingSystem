using BankingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Services.Interfaces
{
    public interface IPdfExportService
    {
        void ExportTransactions(List<Transaction> transactions, string filePath);
    }
}
