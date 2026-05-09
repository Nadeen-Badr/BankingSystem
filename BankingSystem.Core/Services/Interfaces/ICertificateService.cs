using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Services.Interfaces
{
    public interface ICertificateService
    {
        Certificate BuyCertificate(int customerId, decimal price, CertificatePeriod period);

        void UpdateCertificate(int certificateId, decimal price, CertificatePeriod period);

        void DeleteCertificate(int certificateId);
    }
}
