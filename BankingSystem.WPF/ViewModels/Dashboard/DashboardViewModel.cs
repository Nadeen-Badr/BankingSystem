using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Base;
using System;

namespace BankingSystem.WPF.ViewModels.Dashboard
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly AppServices _services;

        public DashboardViewModel(AppServices services)
        {
            _services = services;
            LoadStats();
        }

        private int _totalCustomers;
        public int TotalCustomers
        {
            get => _totalCustomers;
            set => SetProperty(ref _totalCustomers, value);
        }

        private int _totalAccounts;
        public int TotalAccounts
        {
            get => _totalAccounts;
            set => SetProperty(ref _totalAccounts, value);
        }

        private decimal _totalBalance;
        public decimal TotalBalance
        {
            get => _totalBalance;
            set => SetProperty(ref _totalBalance, value);
        }

        private int _totalCertificates;
        public int TotalCertificates
        {
            get => _totalCertificates;
            set => SetProperty(ref _totalCertificates, value);
        }

        private int _totalCreditCards;
        public int TotalCreditCards
        {
            get => _totalCreditCards;
            set => SetProperty(ref _totalCreditCards, value);
        }

        private void LoadStats()
        {
            try
            {
                var stats = _services.ReportService.GetBankStatistics();

                TotalCustomers = stats.TotalCustomers;
                TotalAccounts = stats.TotalAccounts;
                TotalBalance = stats.TotalAssets;
                TotalCertificates = stats.TotalCertificates;
                TotalCreditCards = stats.TotalCreditCards;
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex);
            }
        }
    }
}