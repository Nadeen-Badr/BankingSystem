using BankingSystem.Core.Models;
using BankingSystem.WPF;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace BankingSystem.WPF.ViewModels.Customers
{
    public class ExistingCustomerViewModel : ViewModelBase
    {
        private readonly AppServices _services;

        public ExistingCustomerViewModel(AppServices services)
        {
            _services = services;

            Customers = new ObservableCollection<Customer>();

            LoginCommand = new RelayCommand(_ => ExecuteSafely(Login));
            BackCommand = new RelayCommand(_ => RequestBack?.Invoke());

            LoadCustomers();
        }

        public ObservableCollection<Customer> Customers { get; }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand BackCommand { get; }

        public event Action RequestBack;
        public event Action CustomerLoggedIn;

        private void LoadCustomers()
        {
            var data = _services.CustomerService.GetAllCustomers();

            Customers.Clear();
            foreach (var c in data)
                Customers.Add(c);
        }

        private void Login()
        {
            if (SelectedCustomer == null)
                throw new InvalidOperationException();

            AppSession.SetCustomer(SelectedCustomer.Id, SelectedCustomer.Name);

            CustomerLoggedIn?.Invoke();
        }

        private void ExecuteSafely(Action action)
        {
            try { action(); }
            catch (Exception ex) { ErrorHandler.Handle(ex); }
        }
    }
}