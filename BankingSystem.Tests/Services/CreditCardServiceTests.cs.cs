using BankingSystem.Core.Data;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

[TestClass]
public class CreditCardServiceTests : TestBase
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

    // ----------------------------
    // Helper: Create Customer
    // ----------------------------
    private Customer CreateCustomer()
    {
        var customer = new Customer
        {
            Name = "Test User",
            Age = 30,
            Gender = Gender.Male,
            Address = "Cairo"
        };

        _context.Customers.Add(customer);
        _context.SaveChanges();

        return customer;
    }

    // ----------------------------
    // 1. VALID CARD CREATION
    // ----------------------------
    [TestMethod]
    public void CreateCard_ValidData_ShouldCreateCard()
    {
        // Arrange
        var customer = CreateCustomer();
        var service = new CreditCardService(_context, _loggerMock);

        // Act
        var result = service.CreateCard(customer.Id, 100000);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(100000, result.CashLimit);
        Assert.AreEqual(customer.Id, result.Id);
    }

    // ----------------------------
    // 2. INVALID LIMIT (LOW)
    // ----------------------------
    [TestMethod]
    public void CreateCard_LimitTooLow_ShouldThrowException()
    {
        var customer = CreateCustomer();
        var service = new CreditCardService(_context, _loggerMock);

        bool thrown = false;

        try
        {
            service.CreateCard(customer.Id, 10000); // invalid
        }
        catch (Exception)
        {
            thrown = true;
        }

        Assert.IsTrue(thrown);
    }

    // ----------------------------
    // 3. INVALID LIMIT (HIGH)
    // ----------------------------
    [TestMethod]
    public void CreateCard_LimitTooHigh_ShouldThrowException()
    {
        var customer = CreateCustomer();
        var service = new CreditCardService(_context, _loggerMock);

        bool thrown = false;

        try
        {
            service.CreateCard(customer.Id, 500000); // invalid
        }
        catch (Exception)
        {
            thrown = true;
        }

        Assert.IsTrue(thrown);
    }

    // ----------------------------
    // 4. DUPLICATE CARD
    // ----------------------------
    [TestMethod]
    public void CreateCard_SecondCard_ShouldFail()
    {
        var customer = CreateCustomer();
        var service = new CreditCardService(_context, _loggerMock);

        service.CreateCard(customer.Id, 100000);

        bool thrown = false;

        try
        {
            service.CreateCard(customer.Id, 150000);
        }
        catch (Exception)
        {
            thrown = true;
        }

        Assert.IsTrue(thrown);
    }

    // ----------------------------
    // 5. UPDATE LIMIT SUCCESS
    // ----------------------------
    [TestMethod]
    public void UpdateLimit_Valid_ShouldUpdate()
    {
        var customer = CreateCustomer();
        var service = new CreditCardService(_context, _loggerMock);

        service.CreateCard(customer.Id, 100000);
        service.UpdateLimit(customer.Id, 200000);

        var card = service.GetByCustomer(customer.Id);

        Assert.AreEqual(200000, card.CashLimit);
    }

    // ----------------------------
    // 6. UPDATE INVALID LIMIT
    // ----------------------------
    [TestMethod]
    public void UpdateLimit_Invalid_ShouldThrowException()
    {
        var customer = CreateCustomer();
        var service = new CreditCardService(_context, _loggerMock);

        service.CreateCard(customer.Id, 100000);

        bool thrown = false;

        try
        {
            service.UpdateLimit(customer.Id, 999); // invalid
        }
        catch (Exception)
        {
            thrown = true;
        }

        Assert.IsTrue(thrown);
    }

    // ----------------------------
    // 7. GET CARD
    // ----------------------------
    [TestMethod]
    public void GetByCustomer_ShouldReturnCard()
    {
        var customer = CreateCustomer();
        var service = new CreditCardService(_context, _loggerMock);

        service.CreateCard(customer.Id, 120000);

        var card = service.GetByCustomer(customer.Id);

        Assert.IsNotNull(card);
        Assert.AreEqual(120000, card.CashLimit);
    }
}