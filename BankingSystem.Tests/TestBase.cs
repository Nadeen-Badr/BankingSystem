using System;
using BankingSystem.Core.Data;

public class TestBase
{
    protected BankingDbContext CreateContext()
    {
        return new BankingDbContext();
    }

    protected void ClearDatabase()
    {
        using (var context = new BankingDbContext())
        {
            // WARNING: This deletes the DB every test run
            // Good for learning / small projects
            if (context.Database.Exists())
            {
                context.Database.Delete();
            }

            context.Database.Create();
        }
    }
}