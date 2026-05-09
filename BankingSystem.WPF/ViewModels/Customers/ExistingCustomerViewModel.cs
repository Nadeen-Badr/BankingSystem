using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BankingSystem.WPF.ViewModels.Customers
{
    public class ExistingCustomerViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;

        public ExistingCustomerViewModel(ICustomerService customerService)
        {
            _customerService = customerService;

            Customers = new ObservableCollection<Customer>(_customerService.GetAllCustomers());

            LoginCommand = new RelayCommand(_ => Login());
            BackCommand = new RelayCommand(_ => RequestBack?.Invoke());
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

        private void Login()
        {
            if (SelectedCustomer == null)
                return;

            AppSession.SetCustomer(SelectedCustomer);
            AppSession.SetRole(UserRole.Customer);

            CustomerLoggedIn?.Invoke();
        }
    }
}