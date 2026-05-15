using BankingSystem.Core.Models;
using BankingSystem.WPF;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
namespace BankingSystem.WPF.ViewModels.Customers
{
    public class CustomerDashboardViewModel : ViewModelBase
    {
        private readonly AppServices _services;

        public CustomerDashboardViewModel(AppServices services)
        {
            _services = services;

            ExportTransactionsCommand = new RelayCommand(_ => ExecuteSafely(ExportTransactions));
            RefreshCommand = new RelayCommand(_ => ExecuteSafely(LoadCustomerData));

            Accounts = new ObservableCollection<Account>();
            Transactions = new ObservableCollection<Transaction>();
            Certificates = new ObservableCollection<Certificate>();
            CreditCards = new ObservableCollection<CreditCard>();

            LoadCustomerData();
        }

        public ICommand ExportTransactionsCommand { get; }
        public ICommand RefreshCommand { get; }

        public ObservableCollection<Account> Accounts { get; }
        public ObservableCollection<Transaction> Transactions { get; }
        public ObservableCollection<Certificate> Certificates { get; }
        public ObservableCollection<CreditCard> CreditCards { get; }

        private string _customerName;
        public string CustomerName { get => _customerName; set => SetProperty(ref _customerName, value); }

        private int _customerId;
        public int CustomerId { get => _customerId; set => SetProperty(ref _customerId, value); }

        private decimal _totalBalance;
        public decimal TotalBalance { get => _totalBalance; set => SetProperty(ref _totalBalance, value); }
        public int CertificatesCount => Certificates?.Count ?? 0;
        public int CreditCardsCount => CreditCards?.Count ?? 0;
        private void LoadCustomerData()
        {
            var id = AppSession.CurrentCustomerId;
            if (id == null) return;

            var report = _services.ReportService.GetCustomerReport(id.Value);
            if (report == null) return;

            CustomerName = report.CustomerName;
            CustomerId = id.Value;
            TotalBalance = report.TotalBalance;

            Accounts.Clear();
            foreach (var x in report.Accounts ?? Enumerable.Empty<Account>())
                Accounts.Add(x);

            Transactions.Clear();
            foreach (var x in report.Transactions ?? Enumerable.Empty<Transaction>())
                Transactions.Add(x);

            Certificates.Clear();
            foreach (var x in report.Certificates ?? Enumerable.Empty<Certificate>())
                Certificates.Add(x);

            CreditCards.Clear();
            foreach (var x in report.CreditCards ?? Enumerable.Empty<CreditCard>())
                CreditCards.Add(x);
        }

        private void ExportTransactions()
        {
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "Transactions.pdf"
            );

            _services.PdfExportService.ExportTransactions(Transactions.ToList(), path);
        }

        private void ExecuteSafely(Action action)
        {
            try { action(); }
            catch (Exception ex) { ErrorHandler.Handle(ex); }
        }
    }
}