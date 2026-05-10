using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BankingSystem.WPF.ViewModels.Accounts
{
    public class AccountsViewModel : ViewModelBase
    {
        private readonly IAccountService _accountService;

        public ObservableCollection<Account> Accounts { get; set; }

        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set => SetProperty(ref _selectedAccount, value);
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public ICommand LoadAccountsCommand { get; }
        public ICommand CreateSavingCommand { get; }
        public ICommand CreateSalaryCommand { get; }
        public ICommand DepositCommand { get; }
        public ICommand WithdrawCommand { get; }
        public ICommand CloseAccountCommand { get; }

        public AccountsViewModel(IAccountService accountService)
        {
            _accountService = accountService;

            Accounts = new ObservableCollection<Account>();

            LoadAccountsCommand = new RelayCommand(_ => LoadAccounts());
            CreateSavingCommand = new RelayCommand(_ => CreateSaving());
            CreateSalaryCommand = new RelayCommand(_ => CreateSalary());
            WithdrawCommand = new RelayCommand(_ => Withdraw(), _ => CanExecuteAccountAction());
            DepositCommand = new RelayCommand(_ => Deposit(), _ => CanExecuteAccountAction());
            CloseAccountCommand = new RelayCommand(_ => CloseAccount(), _ => CanExecuteAccountAction());

            LoadAccounts();
        }

        // ================= LOAD =================
        private void LoadAccounts()
        {
            try
            {
                if (AppSession.CurrentCustomerId == null)
                    return;

                var data = _accountService.GetAccountsByCustomer(AppSession.CurrentCustomerId.Value);

                Accounts.Clear();

                foreach (var acc in data)
                    Accounts.Add(acc);
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex);
            }
        }

        // ================= CREATE =================
        private void CreateSaving()
        {
            try
            {
                _accountService.CreateSavingAccount(AppSession.CurrentCustomerId.Value);
                LoadAccounts();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex);
            }
        }

        private void CreateSalary()
        {
            try
            {
                _accountService.CreateSalaryAccount(AppSession.CurrentCustomerId.Value);
                LoadAccounts();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex);
            }
        }

        // ================= TRANSACTIONS =================
        private void Deposit()
        {
            if (SelectedAccount == null) return;

            try
            {
                _accountService.Deposit(SelectedAccount.Id, Amount);
                LoadAccounts();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex);
            }
        }

        private void Withdraw()
        {
            if (SelectedAccount == null) return;

            try
            {
                _accountService.Withdraw(SelectedAccount.Id, Amount);
                LoadAccounts();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex);
            }
        }

        // ================= CLOSE =================
        private void CloseAccount()
        {
            if (SelectedAccount == null) return;

            try
            {
                _accountService.CloseAccount(SelectedAccount.Id);
                LoadAccounts();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex);
            }
        }
        private bool CanExecuteAccountAction()
        {
            return SelectedAccount != null;
        }
    }
}