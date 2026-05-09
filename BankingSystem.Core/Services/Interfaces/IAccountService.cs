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
        void Deposit(int accountId, decimal amount);
        void Withdraw(int accountId, decimal amount);
        List<Account> GetAllAccounts();
    }
}
