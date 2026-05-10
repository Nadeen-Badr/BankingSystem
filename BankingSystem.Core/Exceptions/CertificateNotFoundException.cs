using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace BankingSystem.Core.Exceptions
{
    public class CertificateNotFoundException : Exception
    {
        public CertificateNotFoundException()
            : base("Certificate not found")
        {
        }
    }
}