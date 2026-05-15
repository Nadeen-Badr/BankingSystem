using BankingSystem.Core.Enums;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.ViewModels.Base;
using System;

namespace BankingSystem.WPF.ViewModels.Auth
{
    public class LoginViewModel : ViewModelBase
    {
        public event Action<UserRole> RoleSelected;
        public event Action ShowExistingCustomer;
        public event Action ShowNewCustomer;

        public RelayCommand SelectAdminCommand { get; }
        public RelayCommand SelectExistingCustomerCommand { get; }
        public RelayCommand SelectNewCustomerCommand { get; }

        public LoginViewModel()
        {
            SelectAdminCommand = new RelayCommand(_ =>
                RoleSelected?.Invoke(UserRole.Admin));

            SelectExistingCustomerCommand = new RelayCommand(_ =>
                ShowExistingCustomer?.Invoke());

            SelectNewCustomerCommand = new RelayCommand(_ =>
                ShowNewCustomer?.Invoke());
        }
    }
}