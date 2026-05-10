using BankingSystem.Core.Data;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Exceptions;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public class AccountServiceTests : TestBase
{
    private ILoggerService _loggerMock;
    private AccountService _service;

    [TestInitialize]
    public void SetupTest()
    {
        base.Setup();

        _loggerMock = new Mock<ILoggerService>().Object;
        _service = new AccountService(Context, _loggerMock);
    }

    // -----------------------------
    // Helpers
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

        Context.Customers.Add(customer);
        Context.SaveChanges();

        return customer;
    }

    private Account CreateAccount(decimal balance = 0)
    {
        var customer = CreateCustomer();

        var account = new SavingAccount
        {
            CustomerId = customer.Id,
            Balance = balance,
            AccountType = AccountType.Saving,
            Transactions = new List<Transaction>()
        };

        Context.Accounts.Add(account);
        Context.SaveChanges();

        return account;
    }

    // -----------------------------
    // 1. DEPOSIT SUCCESS
    // -----------------------------
    [TestMethod]
    public void Deposit_ValidAmount_ShouldIncreaseBalance()
    {
        var account = CreateAccount(1000);

        _service.Deposit(account.Id, 500);

        var updated = Context.Accounts.First(a => a.Id == account.Id);

        Assert.AreEqual(1500, updated.Balance);
    }

    // -----------------------------
    // 2. DEPOSIT INVALID AMOUNT
    // -----------------------------
    [TestMethod]
    public void Deposit_InvalidAmount_ShouldThrowException()
    {
        var account = CreateAccount(1000);

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.Deposit(account.Id, -100);
        });
    }

    // -----------------------------
    // 3. WITHDRAW SUCCESS
    // -----------------------------
    [TestMethod]
    public void Withdraw_ValidAmount_ShouldDecreaseBalance()
    {
        var account = CreateAccount(1000);

        _service.Withdraw(account.Id, 400);

        var updated = Context.Accounts.First(a => a.Id == account.Id);

        Assert.AreEqual(600, updated.Balance);
    }

    // -----------------------------
    // 4. WITHDRAW INSUFFICIENT BALANCE
    // -----------------------------
    [TestMethod]
    public void Withdraw_InsufficientBalance_ShouldThrowException()
    {
        var account = CreateAccount(200);

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.Withdraw(account.Id, 500);
        });
    }

    // -----------------------------
    // 5. CREATE SAVING ACCOUNT
    // -----------------------------
    [TestMethod]
    public void CreateSavingAccount_ShouldCreateAccount()
    {
        var customer = CreateCustomer();

        var account = _service.CreateSavingAccount(customer.Id);

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

        var account = _service.CreateSalaryAccount(customer.Id);

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

        _service.CloseAccount(account.Id);

        var deleted = Context.Accounts.Find(account.Id);

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

        var result = _service.GetAllAccounts();

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

        Context.Accounts.Add(account1);
        Context.Accounts.Add(account2);
        Context.SaveChanges();

        var result = _service.GetAccountsByCustomer(customer.Id);

        Assert.AreEqual(2, result.Count);
    }
}