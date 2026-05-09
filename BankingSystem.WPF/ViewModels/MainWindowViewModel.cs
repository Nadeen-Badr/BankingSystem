using BankingSystem.Core.Data;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.Core.Services;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Base;
using BankingSystem.WPF.ViewModels.Auth;
using BankingSystem.WPF.ViewModels.Customers;
using BankingSystem.WPF.ViewModels.Dashboard;
using BankingSystem.WPF.ViewModels.Admin;
using System.Windows.Input;

namespace BankingSystem.WPF.ViewModels.Accounts
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly BankingDbContext _context;
        private readonly ILoggerService _logger;
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;

        private object _currentView;

        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowCustomersCommand { get; }
        public ICommand ShowAccountsCommand { get; }
        public ICommand ShowLogoutCommand { get; }
        public ICommand ShowLogsCommand { get; }

        public bool IsLoggedIn => AppSession.CurrentRole != null;
        public bool IsCustomer => AppSession.CurrentRole == UserRole.Customer;
        public bool IsAdmin => AppSession.CurrentRole == UserRole.Admin;

        public MainWindowViewModel()
        {
            _context = new BankingDbContext();
            _logger = new LoggingService();
            _customerService = new CustomerService(_context, _logger);
            _accountService = new AccountService(_context, _logger);
            AppSession.RoleChanged += OnRoleChanged;

            ShowDashboardCommand = new RelayCommand(_ => ShowDashboard());
            ShowCustomersCommand = new RelayCommand(_ => ShowCustomers());
            ShowAccountsCommand = new RelayCommand(_ => ShowAccounts());
            ShowLogoutCommand = new RelayCommand(_ => Logout());
            ShowLogsCommand = new RelayCommand(_ => ShowLogs());

            // default landing page AFTER login
            ShowDashboard();
        }

        // ================= LOGIN =================
        private void ShowLogin()
        {
            var loginVM = new LoginViewModel();
            loginVM.RoleSelected += OnRoleSelected;

            CurrentView = loginVM;
        }

        private void OnRoleSelected(UserRole role)
        {
            switch (role)
            {
                case UserRole.Admin:
                    ShowDashboard();
                    break;

                case UserRole.Customer:
                    ShowNewCustomer(); // ✅ FIXED FLOW
                    break;
            }
        }

        // ================= ROLE CHANGE =================
        private void OnRoleChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(IsCustomer));
        }

        // ================= NAVIGATION =================

        private void ShowDashboard()
        {
            if (AppSession.CurrentRole == UserRole.Admin)
            {
                CurrentView = new DashboardViewModel();
            }
            else if (AppSession.CurrentRole == UserRole.Customer)
            {
                CurrentView = new CustomerDashboardViewModel(_customerService);
            }
            else
            {
                ShowLogin();
            }
        }
        private void ShowCustomers()
        {
            CurrentView = new CustomersViewModel();
        }

        private void ShowAccounts()
        {
            CurrentView = new AccountsViewModel(_accountService);
        }

        private void ShowLogs()
        {
            CurrentView = new LogsViewModel();
        }

        private void ShowNewCustomer()
        {
            var vm = new NewCustomerViewModel(_customerService, _logger);

            vm.RequestBackToLogin += ShowLogin;

            CurrentView = vm;
        }

        // ================= LOGOUT =================
        private void Logout()
        {
            AppSession.Clear();

            var auth = new BankingSystem.WPF.Views.Auth.AuthWindow();
            auth.DataContext = new AuthWindowViewModel(auth);
            auth.Show();

            // close MainWindow
            System.Windows.Application.Current.Windows[0]?.Close();
        }
    }
}