using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services;
using BankingSystem.Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CertificateServiceTests : TestBase
{
    private FakeLogger _logger;
    private CertificateService _service;

    [TestInitialize]
    public void SetupTest()
    {
        base.Setup();

        _logger = new FakeLogger();
        _service = new CertificateService(Context, _logger);
    }

    // ---------------- HELPERS ----------------

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

    // ---------------- TESTS ----------------

    [TestMethod]
    public void BuyCertificate_Valid_ShouldCreate()
    {
        var customer = CreateCustomer();

        var result = _service.BuyCertificate(customer.Id, 2000, CertificatePeriod.OneYear);

        Assert.IsNotNull(result);
        Assert.AreEqual(2000, result.Price);
        Assert.AreEqual(0.10m, result.InterestRate);
    }

    [TestMethod]
    public void BuyCertificate_InvalidPrice_ShouldThrow()
    {
        var customer = CreateCustomer();

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.BuyCertificate(customer.Id, 900, CertificatePeriod.OneYear);
        });
    }

    [TestMethod]
    public void BuyCertificate_InvalidPeriod_ShouldThrow()
    {
        var customer = CreateCustomer();

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.BuyCertificate(customer.Id, 2000, (CertificatePeriod)999);
        });
    }

    [TestMethod]
    public void BuyCertificate_CustomerNotFound_ShouldThrow()
    {
        Assert.Throws<CustomerNotFoundException>(() =>
        {
            _service.BuyCertificate(999, 2000, CertificatePeriod.OneYear);
        });
    }
}