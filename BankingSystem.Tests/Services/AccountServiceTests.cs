using BankingSystem.Core.Data;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Exceptions;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public class AccountServiceTests : TestBase
{
    private FakeLogger _logger;
    private AccountService _service;

    [TestInitialize]
    public void SetupTest()
    {
        base.Setup();

        _logger = new FakeLogger();
        _service = new AccountService(Context, _logger);
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

    // ---------------- TESTS ----------------

    [TestMethod]
    public void Deposit_Valid_ShouldIncreaseBalance()
    {
        var account = CreateAccount(1000);

        _service.Deposit(account.Id, 500);

        var updated = Context.Accounts.First(a => a.Id == account.Id);

        Assert.AreEqual(1500, updated.Balance);
    }

    [TestMethod]
    public void Deposit_InvalidAmount_ShouldThrow()
    {
        var account = CreateAccount(1000);

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.Deposit(account.Id, -100);
        });
    }

    [TestMethod]
    public void Deposit_AccountNotFound_ShouldThrow()
    {
        Assert.Throws<AccountNotFoundException>(() =>
        {
            _service.Deposit(999, 100);
        });
    }

    [TestMethod]
    public void Withdraw_Valid_ShouldDecreaseBalance()
    {
        var account = CreateAccount(1000);

        _service.Withdraw(account.Id, 400);

        var updated = Context.Accounts.First(a => a.Id == account.Id);

        Assert.AreEqual(600, updated.Balance);
    }

    [TestMethod]
    public void Withdraw_InsufficientBalance_ShouldThrow()
    {
        var account = CreateAccount(200);

        Assert.Throws<InvalidOperationBusinessException>(() =>
        {
            _service.Withdraw(account.Id, 500);
        });
    }

    [TestMethod]
    public void Withdraw_AccountNotFound_ShouldThrow()
    {
        Assert.Throws<AccountNotFoundException>(() =>
        {
            _service.Withdraw(999, 100);
        });
    }

    [TestMethod]
    public void CreateSavingAccount_ShouldCreate()
    {
        var customer = CreateCustomer();

        var account = _service.CreateSavingAccount(customer.Id);

        Assert.IsNotNull(account);
        Assert.AreEqual(AccountType.Saving, account.AccountType);
    }

    [TestMethod]
    public void CreateSalaryAccount_ShouldCreate()
    {
        var customer = CreateCustomer();

        var account = _service.CreateSalaryAccount(customer.Id);

        Assert.IsNotNull(account);
        Assert.AreEqual(AccountType.Salary, account.AccountType);
    }

    [TestMethod]
    public void CloseAccount_ShouldRemove()
    {
        var account = CreateAccount();

        _service.CloseAccount(account.Id);

        var deleted = Context.Accounts.Find(account.Id);

        Assert.IsNull(deleted);
    }

    [TestMethod]
    public void GetAllAccounts_ShouldReturnCorrectCount()
    {
        CreateAccount();
        CreateAccount();

        var result = _service.GetAllAccounts();

        Assert.AreEqual(2, result.Count);
    }

    [TestMethod]
    public void GetAccountsByCustomer_ShouldReturnOnlyThatCustomer()
    {
        var customer = CreateCustomer();

        Context.Accounts.Add(new SavingAccount { CustomerId = customer.Id, Balance = 0 });
        Context.Accounts.Add(new SalaryAccount { CustomerId = customer.Id, Balance = 0 });
        Context.SaveChanges();

        var result = _service.GetAccountsByCustomer(customer.Id);

        Assert.AreEqual(2, result.Count);
    }
}