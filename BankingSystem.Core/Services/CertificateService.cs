using BankingSystem.Core.Data;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Exceptions;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

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
            {
                _logger.Log($"BUY_CERTIFICATE_FAILED | Customer:{customerId} | Reason: Customer not found");
                throw new CustomerNotFoundException();
            }

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

            _logger.Log($"BUY_CERTIFICATE_SUCCESS | Customer:{customerId} | Price:{price} | Period:{period} | Interest:{certificate.InterestRate}");

            return certificate;
        }

        public void UpdateCertificate(int certificateId, decimal price, CertificatePeriod period)
        {
            var existing = _context.Certificates.Find(certificateId);

            if (existing == null)
            {
                _logger.Log($"UPDATE_CERTIFICATE_FAILED | ID:{certificateId} | Reason: Not found");
                throw new CertificateNotFoundException();
            }

            ValidatePrice(price);
            ValidatePeriod(period);

            existing.Price = price;
            existing.Period = period;
            existing.InterestRate = GetInterestRate(period);

            _context.SaveChanges();

            _logger.Log($"UPDATE_CERTIFICATE_SUCCESS | ID:{certificateId} | NewPrice:{price} | NewPeriod:{period}");
        }


        public void DeleteCertificate(int certificateId)
        {
            var cert = _context.Certificates.Find(certificateId);

            if (cert == null)
            {
                _logger.Log($"DELETE_CERTIFICATE_FAILED | ID:{certificateId} | Reason: Not found");
                throw new CertificateNotFoundException();
            }

            _context.Certificates.Remove(cert);
            _context.SaveChanges();

            _logger.Log($"DELETE_CERTIFICATE_SUCCESS | ID:{certificateId}");
        }

        // ---------------- RULES ----------------

        private void ValidatePrice(decimal price)
        {
            if (price < 1000 || price % 1000 != 0)
            {
                _logger.Log($"CERTIFICATE_VALIDATION_FAILED | Reason: Invalid price {price}");
                throw new InvalidOperationBusinessException("Price must be >= 1000 and multiple of 1000");
            }
        }

        private void ValidatePeriod(CertificatePeriod period)
        {
            if (!Enum.IsDefined(typeof(CertificatePeriod), period))
            {
                _logger.Log($"CERTIFICATE_VALIDATION_FAILED | Reason: Invalid period {period}");
                throw new InvalidOperationBusinessException("Invalid certificate period");
            }
        }


        private decimal GetInterestRate(CertificatePeriod period)
        {
            if (period == CertificatePeriod.OneYear)
                return 0.10m;

            if (period == CertificatePeriod.ThreeYears)
                return 0.15m;

            if (period == CertificatePeriod.FiveYears)
                return 0.20m;

            throw new InvalidOperationBusinessException("Invalid certificate period");
        }
        public List<Certificate> GetByCustomer(int customerId)
        {
            return _context.Certificates
                .Where(c => c.CustomerId == customerId)
                .ToList();
        }
    }
     

    }