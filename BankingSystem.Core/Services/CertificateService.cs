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

        public Certificate BuyCertificate(int customerId, decimal price, CertificatePeriod period)
        {
            var customer = _context.Customers.Find(customerId);

            if (customer == null)
                throw new Exception("Customer not found");

            ValidatePrice(price);
            ValidatePeriod(period);

            var certificate = new Certificate
            {
                CustomerId = customerId,
                Price = price,
                Period = period,
                InterestRate = GetInterestRate(period),
                StartDate = DateTime.Now
            };

            _context.Certificates.Add(certificate);
            _context.SaveChanges();

            _logger.Log($"BUY_CERTIFICATE | Customer:{customerId} | Price:{price}");

            return certificate;
        }
        public void UpdateCertificate(int certificateId, decimal price, CertificatePeriod period)
        {
            var existing = _context.Certificates.Find(certificateId);

            if (existing == null)
                throw new Exception("Certificate not found");

            ValidatePrice(price);
            ValidatePeriod(period);

            existing.Price = price;
            existing.Period = period;
            existing.InterestRate = GetInterestRate(period);

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

        private void ValidatePrice(decimal price)
        {
            if (price < 1000 || price % 1000 != 0)
                throw new Exception("Price must be >= 1000 and multiple of 1000");
        }

        private void ValidatePeriod(CertificatePeriod period)
        {
            if (!Enum.IsDefined(typeof(CertificatePeriod), period))
                throw new Exception("Invalid certificate period");
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