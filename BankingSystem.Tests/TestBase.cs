using System;
using BankingSystem.Core.Data;
using BankingSystem.Core.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

public class TestBase
{
    protected BankingDbContext Context;

    [TestInitialize]
    public void Setup()
    {
        Context = new BankingDbContext();

        if (Context.Database.Exists())
        {
            Context.Database.Delete();
        }

        Context.Database.Create();
    }
}