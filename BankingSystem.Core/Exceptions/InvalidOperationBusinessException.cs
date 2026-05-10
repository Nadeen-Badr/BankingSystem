using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Exceptions
{
    public class InvalidOperationBusinessException : BusinessException
    {
        public InvalidOperationBusinessException(string message)
            : base(message)
        {
        }
    }
}