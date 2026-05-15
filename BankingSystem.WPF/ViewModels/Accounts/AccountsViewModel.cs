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
        private readonly AppServices _services;

        public ObservableCollection<Account> Accounts { get; }

        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                SetProperty(ref _selectedAccount, value);

                // refresh buttons (Deposit/Withdraw/Close)
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        // ================= COMMANDS =================
        public ICommand LoadAccountsCommand { get; }
        public ICommand CreateSavingCommand { get; }
        public ICommand CreateSalaryCommand { get; }
        public ICommand DepositCommand { get; }
        public ICommand WithdrawCommand { get; }
        public ICommand CloseAccountCommand { get; }

        public AccountsViewModel(AppServices services)
        {
            _services = services;

            Accounts = new ObservableCollection<Account>();

            LoadAccountsCommand = new RelayCommand(_ => ExecuteSafely(LoadAccounts));
            CreateSavingCommand = new RelayCommand(_ => ExecuteSafely(CreateSaving));
            CreateSalaryCommand = new RelayCommand(_ => ExecuteSafely(CreateSalary));

            DepositCommand = new RelayCommand(_ => ExecuteSafely(Deposit), _ => CanExecuteTransaction());
            WithdrawCommand = new RelayCommand(_ => ExecuteSafely(Withdraw), _ => CanExecuteTransaction());
            CloseAccountCommand = new RelayCommand(_ => ExecuteSafely(CloseAccount), _ => SelectedAccount != null);

            LoadAccounts();
        }

        // ================= SAFE WRAPPER =================
        private void ExecuteSafely(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex);
            }
        }

        // ================= LOAD =================
        private void LoadAccounts()
        {
            try
            {
                if (AppSession.CurrentCustomerId == null)
                    return;

                var data = _services.AccountService.GetAccountsByCustomer(AppSession.CurrentCustomerId.Value);

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
            EnsureCustomer();
            _services.AccountService.CreateSavingAccount(AppSession.CurrentCustomerId.Value);
            LoadAccounts();
        }

        private void CreateSalary()
        {
            EnsureCustomer();
            _services.AccountService.CreateSalaryAccount(AppSession.CurrentCustomerId.Value);
            LoadAccounts();
        }

        // ================= TRANSACTIONS =================
        private void Deposit()
        {
            ValidateTransaction();
            _services.AccountService.Deposit(SelectedAccount.Id, Amount);
            LoadAccounts();
        }

        private void Withdraw()
        {
            ValidateTransaction();
            _services.AccountService.Withdraw(SelectedAccount.Id, Amount);
            LoadAccounts();
        }

        // ================= CLOSE =================
        private void CloseAccount()
        {
            if (SelectedAccount == null)
                throw new InvalidOperationException("No account selected.");

            _services.AccountService.CloseAccount(SelectedAccount.Id);
            LoadAccounts();
        }

        // ================= VALIDATION =================
        private bool CanExecuteTransaction()
        {
            return SelectedAccount != null && Amount > 0;
        }

        private void EnsureCustomer()
        {
            if (AppSession.CurrentCustomerId == null)
                throw new InvalidOperationException("No customer selected.");
        }

        private void ValidateTransaction()
        {
            if (SelectedAccount == null)
                throw new InvalidOperationException("No account selected.");

            if (Amount <= 0)
                throw new InvalidOperationException("Amount must be greater than zero.");
        }
    }
}