using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace BankingSystem.Core.Exceptions
{
    public class CreditCardNotFoundException : Exception
    {
        public CreditCardNotFoundException()
            : base("Credit card not found")
        {
        }
    }
}