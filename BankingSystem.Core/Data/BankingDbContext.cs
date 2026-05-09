using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.Core.Models;
using System.Data.Entity;


namespace BankingSystem.Core.Data
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext()
            : base("BankingDb") // connection string name
        {
        }

        // Tables
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<SavingAccount> SavingAccounts { get; set; }
        public DbSet<SalaryAccount> SalaryAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CUSTOMER → ACCOUNTS (1 : Many)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Accounts)
                .WithRequired(a => a.Customer)
                .HasForeignKey(a => a.CustomerId);

            // CUSTOMER → CERTIFICATES (1 : Many)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Certificates)
                .WithRequired(cer => cer.Customer)
                .HasForeignKey(cer => cer.CustomerId);

            // CUSTOMER → CREDIT CARD (1 : 1)
            modelBuilder.Entity<Customer>()
                .HasOptional(c => c.CreditCard)
                .WithRequired(cc => cc.Customer);

            // ACCOUNT → TRANSACTIONS (1 : Many)
            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithRequired(t => t.Account)
                .HasForeignKey(t => t.AccountId);

         
        }
    }


}
