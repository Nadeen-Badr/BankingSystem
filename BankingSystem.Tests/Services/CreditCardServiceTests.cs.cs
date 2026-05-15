using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services;
using BankingSystem.Core.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CreditCardServiceTests : TestBase
{
    private FakeLogger _logger;
    private CreditCardService _service;

    [TestInitialize]
    public void SetupTest()
    {
        base.Setup();

        _logger = new FakeLogger();
        _service = new CreditCardService(Context, _logger);
    }

    // ---------------- HELPERS ----------------

    private Customer CreateCustomer()
    {
        var customer = new Customer
        {
            Name = "Test User",
            Age = 30,
            Gender = Gender.Male,
            Address = "Cairo"
        };

        Context.Customers.Add(customer);
        Context.SaveChanges();

        return customer;
    }

    // ---------------- TESTS ----------------

    [TestMethod]
    public void CreateCard_Valid_ShouldCreate()
    {
        var customer = CreateCustomer();

        var result = _service.CreateCard(customer.Id, 100000);

        Assert.IsNotNull(result);
        Assert.AreEqual(100000, result.CashLimit);
        Assert.AreEqual(customer.Id, result.Id);
    }

    [TestMethod]
    public void CreateCard_LimitTooLow_ShouldThrow()
    {
        var customer = CreateCustomer();

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.CreateCard(customer.Id, 10000);
        });
    }

    [TestMethod]
    public void CreateCard_LimitTooHigh_ShouldThrow()
    {
        var customer = CreateCustomer();

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.CreateCard(customer.Id, 500000);
        });
    }

    [TestMethod]
    public void CreateCard_Duplicate_ShouldThrow()
    {
        var customer = CreateCustomer();

        _service.CreateCard(customer.Id, 100000);

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.CreateCard(customer.Id, 150000);
        });
    }

    [TestMethod]
    public void UpdateLimit_Valid_ShouldUpdate()
    {
        var customer = CreateCustomer();

        _service.CreateCard(customer.Id, 100000);
        _service.UpdateLimit(customer.Id, 200000);

        var card = _service.GetByCustomer(customer.Id);

        Assert.AreEqual(200000, card.CashLimit);
    }

    [TestMethod]
    public void UpdateLimit_Invalid_ShouldThrow()
    {
        var customer = CreateCustomer();

        _service.CreateCard(customer.Id, 100000);

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.UpdateLimit(customer.Id, 999);
        });
    }

    [TestMethod]
    public void GetByCustomer_ShouldReturnCard()
    {
        var customer = CreateCustomer();

        _service.CreateCard(customer.Id, 120000);

        var card = _service.GetByCustomer(customer.Id);

        Assert.IsNotNull(card);
        Assert.AreEqual(120000, card.CashLimit);
    }

    [TestMethod]
    public void UpdateLimit_CardNotFound_ShouldThrow()
    {
        Assert.Throws<CreditCardNotFoundException>(() =>
        {
            _service.UpdateLimit(999, 100000);
        });
    }
}