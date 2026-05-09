using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BankingSystem.Core.Data;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BankingSystem.WPF.ViewModels.Accounts
{
    public class AccountsViewModel : ViewModelBase
    {
        private readonly AccountService _accountService;

        public ObservableCollection<Account> Accounts { get; set; }

        private int _accountId;
        private decimal _amount;

        public int AccountId
        {
            get => _accountId;
            set => SetProperty(ref _accountId, value);
        }

        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public ICommand DepositCommand { get; }
        public ICommand WithdrawCommand { get; }

        public AccountsViewModel()
        {
            var context = new BankingDbContext();
            var logger = new LoggingService();

            _accountService = new AccountService(context, logger);

            LoadAccounts();

            DepositCommand = new RelayCommand(_ => Deposit());
            WithdrawCommand = new RelayCommand(_ => Withdraw());
        }

        private void LoadAccounts()
        {
            Accounts = new ObservableCollection<Account>(
                _accountService.GetAllAccounts()
            );
        }

        private void Deposit()
        {
            _accountService.Deposit(AccountId, Amount);
            LoadAccounts();
        }

        private void Withdraw()
        {
            _accountService.Withdraw(AccountId, Amount);
            LoadAccounts();
        }
    }
}
