using BankingSystem.Core.Data;
using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.ViewModels.Base;
using BankingSystem.WPF.ViewModels.Customers;
using BankingSystem.WPF.ViewModels.Accounts;
using System.Windows;

namespace BankingSystem.WPF.ViewModels.Auth
{
    public class AuthWindowViewModel : ViewModelBase
    {
        private readonly Window _authWindow;

        // 🔥 SINGLE SHARED SERVICES (IMPORTANT FIX)
        private readonly BankingDbContext _context;
        private readonly ILoggerService _logger;
        private readonly ICustomerService _customerService;

        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public AuthWindowViewModel(Window authWindow)
        {
            _authWindow = authWindow;

            // ✅ INIT SERVICES ONCE
            _context = new BankingDbContext();
            _logger = new LoggingService();
            _customerService = new CustomerService(_context, _logger);

            ShowRoleSelection();
        }

        private void ShowRoleSelection()
        {
            var vm = new LoginViewModel();

            vm.RoleSelected += OnRoleSelected;
            vm.ShowExistingCustomer += ShowExistingCustomer;
            vm.ShowNewCustomer += ShowNewCustomer;

            CurrentView = vm;
        }

        // ================= ROLE HANDLING =================
        private void OnRoleSelected(UserRole role)
        {
            if (role == UserRole.Admin)
                OpenMainWindow();

            else if (role == UserRole.Customer)
                ShowExistingCustomer(); // OR NewCustomer depending on flow
        }

        // ================= CUSTOMER FLOWS =================

        private void ShowNewCustomer()
        {
            var vm = new NewCustomerViewModel(_customerService, _logger);

            vm.RequestBackToLogin += ShowRoleSelection;

            CurrentView = vm;
        }

        private void ShowExistingCustomer()
        {
            var vm = new ExistingCustomerViewModel(
                new CustomerService(new BankingDbContext(), new LoggingService())
            );

            vm.CustomerLoggedIn += OpenMainWindow;
            vm.RequestBack += ShowRoleSelection;

            CurrentView = vm;
        }

        // ================= MAIN WINDOW =================
        private void OpenMainWindow()
        {
            var main = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };

            main.Show();

            _authWindow.Close();
        }
   
    }
}