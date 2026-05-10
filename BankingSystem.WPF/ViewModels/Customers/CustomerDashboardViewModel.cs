using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace BankingSystem.WPF.ViewModels.Customers
{
    public class CustomerDashboardViewModel : ViewModelBase
    {
        private readonly IReportService _reportService;

        public CustomerDashboardViewModel(IReportService reportService)
        {
            _reportService = reportService;
            LoadCustomerData();
        }

        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set => SetProperty(ref _customerName, value);
        }

        private int _customerId;
        public int CustomerId
        {
            get => _customerId;
            set => SetProperty(ref _customerId, value);
        }

        private decimal _totalBalance;
        public decimal TotalBalance
        {
            get => _totalBalance;
            set => SetProperty(ref _totalBalance, value);
        }

        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<Transaction> Transactions { get; set; }
        public ObservableCollection<Certificate> Certificates { get; set; }
        public ObservableCollection<CreditCard> CreditCards { get; set; }

        // ✅ NEW COUNTERS
        public int CertificatesCount => Certificates?.Count ?? 0;
        public int CreditCardsCount => CreditCards?.Count ?? 0;

        private void LoadCustomerData()
        {
            var customerId = AppSession.CurrentCustomer?.Id;
            if (customerId == null) return;

            var report = _reportService.GetCustomerReport(customerId.Value);
            if (report == null) return;

            CustomerName = report.CustomerName;
            CustomerId = customerId.Value;

            Accounts = new ObservableCollection<Account>(report.Accounts ?? new List<Account>());
            Transactions = new ObservableCollection<Transaction>(report.Transactions ?? new List<Transaction>());
            Certificates = new ObservableCollection<Certificate>(report.Certificates ?? new List<Certificate>());
            CreditCards = new ObservableCollection<CreditCard>(report.CreditCards ?? new List<CreditCard>());

            TotalBalance = report.TotalBalance;
        }
    }
}