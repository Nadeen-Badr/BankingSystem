using BankingSystem.Core.Enums;
using BankingSystem.Core.Exceptions;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

[TestClass]
public class CreditCardServiceTests : TestBase
{
    private ILoggerService _loggerMock;
    private CreditCardService _service;

    [TestInitialize]
    public void SetupTest()
    {
        base.Setup();

        _loggerMock = new Mock<ILoggerService>().Object;
        _service = new CreditCardService(Context, _loggerMock);
    }

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

    // ---------------- VALID ----------------
    [TestMethod]
    public void CreateCard_ValidData_ShouldCreateCard()
    {
        var customer = CreateCustomer();

        var result = _service.CreateCard(customer.Id, 100000);

        Assert.IsNotNull(result);
        Assert.AreEqual(100000, result.CashLimit);
        Assert.AreEqual(customer.Id, result.Id);
    }

    // ---------------- INVALID LOW ----------------
    [TestMethod]
    public void CreateCard_LimitTooLow_ShouldThrow()
    {
        var customer = CreateCustomer();

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.CreateCard(customer.Id, 10000);
        });
    }

    // ---------------- INVALID HIGH ----------------
    [TestMethod]
    public void CreateCard_LimitTooHigh_ShouldThrow()
    {
        var customer = CreateCustomer();

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.CreateCard(customer.Id, 500000);
        });
    }

    // ---------------- DUPLICATE ----------------
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

    // ---------------- UPDATE VALID ----------------
    [TestMethod]
    public void UpdateLimit_Valid_ShouldUpdate()
    {
        var customer = CreateCustomer();

        _service.CreateCard(customer.Id, 100000);
        _service.UpdateLimit(customer.Id, 200000);

        var card = _service.GetByCustomer(customer.Id);

        Assert.AreEqual(200000, card.CashLimit);
    }

    // ---------------- UPDATE INVALID ----------------
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

    // ---------------- GET ----------------
    [TestMethod]
    public void GetByCustomer_ShouldReturnCard()
    {
        var customer = CreateCustomer();

        _service.CreateCard(customer.Id, 120000);

        var card = _service.GetByCustomer(customer.Id);

        Assert.IsNotNull(card);
        Assert.AreEqual(120000, card.CashLimit);
    }
}