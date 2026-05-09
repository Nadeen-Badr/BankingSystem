using BankingSystem.Core.Data;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BankingSystem.Core.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly BankingDbContext _context;
        private readonly ILoggerService _logger;

        public CertificateService(BankingDbContext context, ILoggerService logger)
        {
            _context = context;
            _logger = logger;
        }

        public Certificate BuyCertificate(int customerId, Certificate certificate)
        {
            var customer = _context.Customers.Find(customerId);

            if (customer == null)
                throw new Exception("Customer not found");

            if (!IsValidPrice(certificate.Price))
                throw new Exception("Invalid certificate price");

            certificate.InterestRate = GetInterestRate(certificate.Period);

            certificate.CustomerId = customerId;

            _context.Certificates.Add(certificate);
            _context.SaveChanges();

            _logger.Log($"BUY_CERTIFICATE | Customer:{customerId} | Price:{certificate.Price}");

            return certificate;
        }

        public void UpdateCertificate(int certificateId, Certificate certificate)
        {
            var existing = _context.Certificates.Find(certificateId);

            if (existing == null)
                throw new Exception("Certificate not found");

            if (!IsValidPrice(certificate.Price))
                throw new Exception("Invalid certificate price");

            existing.Price = certificate.Price;
            existing.Period = certificate.Period;
            existing.InterestRate = GetInterestRate(certificate.Period);

            _context.SaveChanges();

            _logger.Log($"UPDATE_CERTIFICATE | ID:{certificateId}");
        }

        public void DeleteCertificate(int certificateId)
        {
            var cert = _context.Certificates.Find(certificateId);

            if (cert == null)
                throw new Exception("Certificate not found");

            _context.Certificates.Remove(cert);
            _context.SaveChanges();

            _logger.Log($"DELETE_CERTIFICATE | ID:{certificateId}");
        }

        // ---------------- PRIVATE BUSINESS RULES ----------------

        private bool IsValidPrice(decimal price)
        {
            return price >= 1000 && price % 1000 == 0;
        }

        private decimal GetInterestRate(CertificatePeriod period)
        {
            if (period == CertificatePeriod.OneYear)
                return 0.10m;

            if (period == CertificatePeriod.ThreeYears)
                return 0.15m;

            if (period == CertificatePeriod.FiveYears)
                return 0.20m;

            throw new Exception("Invalid certificate period");
        }
    }
}