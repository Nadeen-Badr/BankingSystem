using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Exceptions
{
    public class AccountNotFoundException : BusinessException
    {
        public AccountNotFoundException()
            : base("Account not found")
        {
        }
    }
}