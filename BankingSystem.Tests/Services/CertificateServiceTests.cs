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
    private CertificateService _service;

    [TestInitialize]
    public void SetupTest()
    {
        base.Setup();

        _loggerMock = new Mock<ILoggerService>().Object;
        _service = new CertificateService(Context, _loggerMock);
    }

    // -----------------------------
    // Helper
    // -----------------------------
    private Customer CreateCustomer()
    {
        var customer = new Customer
        {
            Name = "Test User",
            Age = 25,
            Gender = Gender.Male,
            Address = "Cairo"
        };

        Context.Customers.Add(customer);
        Context.SaveChanges();

        return customer;
    }

    // -----------------------------
    // 1. VALID BUY CERTIFICATE
    // -----------------------------
    [TestMethod]
    public void BuyCertificate_ValidData_ShouldCreateCertificate()
    {
        var customer = CreateCustomer();

        var result = _service.BuyCertificate(
            customer.Id,
            2000,
            CertificatePeriod.OneYear);

        Assert.IsNotNull(result);
        Assert.AreEqual(2000, result.Price);
        Assert.AreEqual(0.10m, result.InterestRate);
    }

    // -----------------------------
    // 2. INVALID PRICE
    // -----------------------------
    [TestMethod]
    public void BuyCertificate_InvalidPrice_ShouldThrowException()
    {
        var customer = CreateCustomer();

        Assert.Throws<Exception>(() =>
        {
            _service.BuyCertificate(customer.Id, 900, CertificatePeriod.OneYear);
        });
    }

    // -----------------------------
    // 3. INVALID PERIOD
    // -----------------------------
    [TestMethod]
    public void BuyCertificate_InvalidPeriod_ShouldThrowException()
    {
        var customer = CreateCustomer();

        Assert.Throws<Exception>(() =>
        {
            _service.BuyCertificate(customer.Id, 2000, (CertificatePeriod)999);
        });
    }
}