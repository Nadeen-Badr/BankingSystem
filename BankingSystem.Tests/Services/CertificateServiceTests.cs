using BankingSystem.Core.Data;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

[TestClass]
public class CertificateServiceTests : TestBase
{
    private ILoggerService _loggerMock;
    private BankingDbContext _context;

    [TestInitialize]
    public void Setup()
    {
        ClearDatabase();
        _loggerMock = new Mock<ILoggerService>().Object;
        _context = CreateContext();
    }

    [TestMethod]
    public void BuyCertificate_ValidData_ShouldCreateCertificate()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Test User",
            Age = 25,
            Gender = Gender.Male,
            Address = "Cairo"
        };

        _context.Customers.Add(customer);
        _context.SaveChanges();

        var service = new CertificateService(_context, _loggerMock);

        // Act
        var result = service.BuyCertificate(
            customer.Id,
            2000,
            CertificatePeriod.OneYear);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2000, result.Price);
        Assert.AreEqual(0.10m, result.InterestRate);
    }

    [TestMethod]
    public void BuyCertificate_InvalidPrice_ShouldThrowException()
    {
        var customer = new Customer
        {
            Name = "Test User",
            Age = 25,
            Gender = Gender.Male,
            Address = "Cairo"
        };

        _context.Customers.Add(customer);
        _context.SaveChanges();

        var service = new CertificateService(_context, _loggerMock);

        bool exceptionThrown = false;

        try
        {
            service.BuyCertificate(customer.Id, 900, CertificatePeriod.OneYear);
        }
        catch (Exception)
        {
            exceptionThrown = true;
        }

        Assert.IsTrue(exceptionThrown, "Expected exception was not thrown for invalid price");
    }

    [TestMethod]
    public void BuyCertificate_InvalidPeriod_ShouldThrowException()
    {
        var customer = new Customer
        {
            Name = "Test User",
            Age = 25,
            Gender = Gender.Male,
            Address = "Cairo"
        };

        _context.Customers.Add(customer);
        _context.SaveChanges();

        var service = new CertificateService(_context, _loggerMock);

        bool exceptionThrown = false;

        try
        {
            service.BuyCertificate(customer.Id, 2000, (CertificatePeriod)999);
        }
        catch (Exception)
        {
            exceptionThrown = true;
        }

        Assert.IsTrue(exceptionThrown, "Expected exception was not thrown for invalid period");
    }
}