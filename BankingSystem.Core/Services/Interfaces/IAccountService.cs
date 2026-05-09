using BankingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Services.Interfaces
{
    public interface IAccountService
    {
        Account CreateSavingAccount(int customerId);
        Account CreateSalaryAccount(int customerId);

        void Deposit(int accountId, decimal amount);
        void Withdraw(int accountId, decimal amount);

        void CloseAccount(int accountId);

        List<Account> GetAccountsByCustomer(int customerId);
    }
}
