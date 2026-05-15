using BankingSystem.Core.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;

public class TestBase
{
    protected BankingDbContext Context;

    [TestInitialize]
    public void Setup()
    {
        Database.SetInitializer(new DropCreateDatabaseAlways<BankingDbContext>());

        Context = new BankingDbContext();
        Context.Database.Initialize(true);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Context.Dispose();
    }
}