using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Linq;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.ViewModels.Base;

namespace BankingSystem.WPF.ViewModels.Customers
{
    public class CustomerDashboardViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;

        public CustomerDashboardViewModel(ICustomerService customerService)
        {
            _customerService = customerService;

            LoadCustomerData();
        }

        // ================= CUSTOMER INFO =================
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }

        // ================= FINANCIAL DATA =================
        public decimal TotalBalance { get; set; }

        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<Transaction> Transactions { get; set; }

        // ================= LOAD DATA =================
        private void LoadCustomerData()
        {
            var customer = _customerService
                .GetAllCustomers()
                .FirstOrDefault(c => c.Id == AppSession.CurrentCustomer?.Id);

            if (customer == null)
                return;

            CustomerName = customer.Name;
            CustomerId = customer.Id;

            Accounts = new ObservableCollection<Account>(customer.Accounts ?? new List<Account>());

            Transactions = new ObservableCollection<Transaction>(
                customer.Accounts?
                    .SelectMany(a => a.Transactions ?? new List<Transaction>())
                    .OrderByDescending(t => t.Date)
                    .Take(10)
            );

            TotalBalance = Accounts.Sum(a => a.Balance);
        }
    }
}