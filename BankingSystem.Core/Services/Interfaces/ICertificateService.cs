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
        Certificate BuyCertificate(int customerId, Certificate certificate);
        void UpdateCertificate(int certificateId, Certificate certificate);
        void DeleteCertificate(int certificateId);
    }
}
