using BankingSystem.Core.Enums;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Admin;
using BankingSystem.WPF.ViewModels.Auth;
using BankingSystem.WPF.ViewModels.Base;
using BankingSystem.WPF.ViewModels.Customers;
using BankingSystem.WPF.ViewModels.Dashboard;
using BankingSystem.WPF.ViewModels.Services;
using System.Windows.Input;

namespace BankingSystem.WPF.ViewModels.Accounts
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly AppServices _services;

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        // ================= COMMANDS =================
        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowAccountsCommand { get; }
        public ICommand ShowLogoutCommand { get; }
        public ICommand ShowLogsCommand { get; }
        public ICommand ShowServicesCommand { get; }
        public ICommand ShowProfileCommand { get; }

        // ================= FLAGS =================
        public bool IsLoggedIn => AppSession.CurrentRole != null;
        public bool IsCustomer => AppSession.CurrentRole == UserRole.Customer;
        public bool IsAdmin => AppSession.CurrentRole == UserRole.Admin;

        public MainWindowViewModel()
        {
            _services = new AppServices();

            AppSession.RoleChanged += OnRoleChanged;

            ShowDashboardCommand = new RelayCommand(_ => ShowDashboard());
            ShowAccountsCommand = new RelayCommand(_ => ShowAccounts());
            ShowLogoutCommand = new RelayCommand(_ => Logout());
            ShowLogsCommand = new RelayCommand(_ => ShowLogs());
            ShowServicesCommand = new RelayCommand(_ => ShowServices());
            ShowProfileCommand = new RelayCommand(_ => ShowProfile());

            // IMPORTANT: default screen only (NO LOGIN HERE)
            ShowDashboard();
        }

        // ================= ROLE UPDATE =================
        private void OnRoleChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(IsCustomer));

            ShowDashboard();
        }

        // ================= NAVIGATION =================

        private void ShowDashboard()
        {
            if (AppSession.CurrentRole == UserRole.Admin)
            {
                CurrentView = new DashboardViewModel(_services);
            }
            else if (AppSession.CurrentRole == UserRole.Customer)
            {
                CurrentView = new CustomerDashboardViewModel(_services);
            }
        }

        private void ShowAccounts()
        {
            CurrentView = new AccountsViewModel(_services);
        }

        private void ShowLogs()
        {
            CurrentView = new LogsViewModel(_services.Logger);
        }

        private void ShowServices()
        {
            CurrentView = new ServicesViewModel(_services);
        }

        private void ShowProfile()
        {
            CurrentView = new EditCustomerViewModel(_services);
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