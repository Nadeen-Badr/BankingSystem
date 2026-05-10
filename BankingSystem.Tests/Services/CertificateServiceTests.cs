using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

    // ---------------- VALID ----------------
    [TestMethod]
    public void BuyCertificate_ValidData_ShouldCreateCertificate()
    {
        var customer = CreateCustomer();

        var result = _service.BuyCertificate(customer.Id, 2000, CertificatePeriod.OneYear);

        Assert.IsNotNull(result);
        Assert.AreEqual(2000, result.Price);
        Assert.AreEqual(0.10m, result.InterestRate);
    }

    // ---------------- INVALID PRICE ----------------
    [TestMethod]
    public void BuyCertificate_InvalidPrice_ShouldThrow()
    {
        var customer = CreateCustomer();

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.BuyCertificate(customer.Id, 900, CertificatePeriod.OneYear);
        });
    }

    // ---------------- INVALID PERIOD ----------------
    [TestMethod]
    public void BuyCertificate_InvalidPeriod_ShouldThrow()
    {
        var customer = CreateCustomer();

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.BuyCertificate(customer.Id, 2000, (CertificatePeriod)999);
        });
    }
}