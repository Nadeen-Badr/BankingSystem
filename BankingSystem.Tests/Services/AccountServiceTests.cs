using BankingSystem.Core.Data;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

[TestClass]
public class AccountServiceTests : TestBase
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

    // -----------------------------
    // Helper: Create Customer
    // -----------------------------
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

    // -----------------------------
    // Helper: Create Account
    // -----------------------------
    private Account CreateAccount(decimal balance = 0)
    {
        var customer = CreateCustomer();

        var account = new SavingAccount
        {
            CustomerId = customer.Id,
            Balance = balance,
            AccountType = AccountType.Saving
        };

        _context.Accounts.Add(account);
        _context.SaveChanges();

        return account;
    }

    // -----------------------------
    // 1. DEPOSIT SUCCESS
    // -----------------------------
    [TestMethod]
    public void Deposit_ValidAmount_ShouldIncreaseBalance()
    {
        var account = CreateAccount(1000);

        var service = new AccountService(_context, _loggerMock);

        service.Deposit(account.Id, 500);

        var updated = _context.Accounts.Find(account.Id);

        Assert.AreEqual(1500, updated.Balance);
    }

    // -----------------------------
    // 2. DEPOSIT INVALID AMOUNT
    // -----------------------------
    [TestMethod]
    public void Deposit_InvalidAmount_ShouldThrowException()
    {
        var account = CreateAccount(1000);
        var service = new AccountService(_context, _loggerMock);

        bool thrown = false;

        try
        {
            service.Deposit(account.Id, -100);
        }
        catch (Exception)
        {
            thrown = true;
        }

        Assert.IsTrue(thrown);
    }

    // -----------------------------
    // 3. WITHDRAW SUCCESS
    // -----------------------------
    [TestMethod]
    public void Withdraw_ValidAmount_ShouldDecreaseBalance()
    {
        var account = CreateAccount(1000);
        var service = new AccountService(_context, _loggerMock);

        service.Withdraw(account.Id, 400);

        var updated = _context.Accounts.Find(account.Id);

        Assert.AreEqual(600, updated.Balance);
    }

    // -----------------------------
    // 4. WITHDRAW INSUFFICIENT BALANCE
    // -----------------------------
    [TestMethod]
    public void Withdraw_InsufficientBalance_ShouldThrowException()
    {
        var account = CreateAccount(200);
        var service = new AccountService(_context, _loggerMock);

        bool thrown = false;

        try
        {
            service.Withdraw(account.Id, 500);
        }
        catch (Exception)
        {
            thrown = true;
        }

        Assert.IsTrue(thrown);
    }

    // -----------------------------
    // 5. CREATE SAVING ACCOUNT
    // -----------------------------
    [TestMethod]
    public void CreateSavingAccount_ShouldCreateAccount()
    {
        var customer = CreateCustomer();
        var service = new AccountService(_context, _loggerMock);

        var account = service.CreateSavingAccount(customer.Id);

        Assert.IsNotNull(account);
        Assert.AreEqual(AccountType.Saving, account.AccountType);
    }

    // -----------------------------
    // 6. CREATE SALARY ACCOUNT
    // -----------------------------
    [TestMethod]
    public void CreateSalaryAccount_ShouldCreateAccount()
    {
        var customer = CreateCustomer();
        var service = new AccountService(_context, _loggerMock);

        var account = service.CreateSalaryAccount(customer.Id);

        Assert.IsNotNull(account);
        Assert.AreEqual(AccountType.Salary, account.AccountType);
    }

    // -----------------------------
    // 7. CLOSE ACCOUNT
    // -----------------------------
    [TestMethod]
    public void CloseAccount_ShouldRemoveAccount()
    {
        var account = CreateAccount();
        var service = new AccountService(_context, _loggerMock);

        service.CloseAccount(account.Id);

        var deleted = _context.Accounts.Find(account.Id);

        Assert.IsNull(deleted);
    }

    // -----------------------------
    // 8. GET ALL ACCOUNTS
    // -----------------------------
    [TestMethod]
    public void GetAllAccounts_ShouldReturnList()
    {
        CreateAccount();
        CreateAccount();

        var service = new AccountService(_context, _loggerMock);

        var result = service.GetAllAccounts();

        Assert.IsTrue(result.Count >= 2);
    }

    // -----------------------------
    // 9. GET BY CUSTOMER
    // -----------------------------
    [TestMethod]
    public void GetAccountsByCustomer_ShouldReturnOnlyCustomerAccounts()
    {
        var customer = CreateCustomer();

        var account1 = new SavingAccount { CustomerId = customer.Id, Balance = 0 };
        var account2 = new SalaryAccount { CustomerId = customer.Id, Balance = 0 };

        _context.Accounts.Add(account1);
        _context.Accounts.Add(account2);
        _context.SaveChanges();

        var service = new AccountService(_context, _loggerMock);

        var result = service.GetAccountsByCustomer(customer.Id);

        Assert.AreEqual(2, result.Count);
    }
}