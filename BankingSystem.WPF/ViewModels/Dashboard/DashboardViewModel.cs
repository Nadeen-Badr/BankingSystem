using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSystem.WPF.ViewModels.Base;
using BankingSystem.Core.Data;
using BankingSystem.Core.Services;

namespace BankingSystem.WPF.ViewModels.Dashboard
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly ReportService _reportService;

        private int _totalCustomers;
        private int _totalAccounts;
        private decimal _totalBalance;
        private int _totalCertificates;
        private int _totalCreditCards;

        public int TotalCustomers
        {
            get => _totalCustomers;
            set => SetProperty(ref _totalCustomers, value);
        }

        public int TotalAccounts
        {
            get => _totalAccounts;
            set => SetProperty(ref _totalAccounts, value);
        }

        public decimal TotalBalance
        {
            get => _totalBalance;
            set => SetProperty(ref _totalBalance, value);
        }

        public int TotalCertificates
        {
            get => _totalCertificates;
            set => SetProperty(ref _totalCertificates, value);
        }

        public int TotalCreditCards
        {
            get => _totalCreditCards;
            set => SetProperty(ref _totalCreditCards, value);
        }

        public DashboardViewModel()
        {
            var context = new BankingDbContext();
            _reportService = new ReportService(context);

            LoadStats();
        }

        private void LoadStats()
        {
            TotalCustomers = _reportService.GetTotalCustomers();
            TotalAccounts = _reportService.GetTotalAccounts();
            //TotalBalance = _reportService.GetTotalBalance();
            TotalCertificates = _reportService.GetTotalCertificates();
            TotalCreditCards = _reportService.GetTotalCreditCards();
        }
    }
}