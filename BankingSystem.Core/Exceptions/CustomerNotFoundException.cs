using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Exceptions
{
    public class CustomerNotFoundException : BusinessException
    {
        public CustomerNotFoundException()
            : base("Customer not found")
        {
        }
    }
}