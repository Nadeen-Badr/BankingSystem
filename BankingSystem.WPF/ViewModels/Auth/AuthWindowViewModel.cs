using BankingSystem.Core.Enums;
using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Accounts;
using BankingSystem.WPF.ViewModels.Base;
using BankingSystem.WPF.ViewModels.Customers;
using System;
using System.Windows;

namespace BankingSystem.WPF.ViewModels.Auth
{
    public class AuthWindowViewModel : ViewModelBase
    {
        private readonly Window _authWindow;
        private readonly AppServices _services;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public AuthWindowViewModel(Window authWindow)
        {
            _authWindow = authWindow;
            _services = new AppServices();

            ShowRoleSelection();
        }

        // ================= ENTRY SCREEN =================
        private void ShowRoleSelection()
        {
            var vm = new LoginViewModel();

            vm.RoleSelected += OnRoleSelected;
            vm.ShowExistingCustomer += ShowExistingCustomer;
            vm.ShowNewCustomer += ShowNewCustomer;

            CurrentView = vm;
        }

        // ================= ROLE SELECT =================
        private void OnRoleSelected(UserRole role)
        {
            AppSession.SetRole(role);

            // Admin OR Customer BOTH go to MainWindow
            OpenMainWindow();
        }

        // ================= CUSTOMER FLOW =================
        private void ShowNewCustomer()
        {
            var vm = new NewCustomerViewModel(_services);

            vm.RequestBackToLogin += ShowRoleSelection;
            CurrentView = vm;
        }

        private void ShowExistingCustomer()
        {
            var vm = new ExistingCustomerViewModel(_services);

            vm.CustomerLoggedIn += OpenMainWindow;
            vm.RequestBack += ShowRoleSelection;

            CurrentView = vm;
        }

        // ================= OPEN MAIN =================
        private void OpenMainWindow()
        {
            var main = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };

            main.Show();

            _authWindow?.Close();
        }
    }
}