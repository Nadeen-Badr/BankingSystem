using BankingSystem.Core.Enums;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Windows.Input;

namespace BankingSystem.WPF.ViewModels.Auth
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand SelectAdminCommand { get; }
        public ICommand SelectExistingCustomerCommand { get; }
        public ICommand SelectNewCustomerCommand { get; }

        public event Action<UserRole> RoleSelected;
        public event Action ShowExistingCustomer;
        public event Action ShowNewCustomer;

        public LoginViewModel()
        {
            SelectAdminCommand = new RelayCommand(_ => SelectRole(UserRole.Admin));

            SelectExistingCustomerCommand =
                new RelayCommand(_ => ShowExistingCustomer?.Invoke());

            SelectNewCustomerCommand =
                new RelayCommand(_ => ShowNewCustomer?.Invoke());
        }

        private void SelectRole(UserRole role)
        {
            AppSession.SetRole(role);
            RoleSelected?.Invoke(role);
        }
    }
}