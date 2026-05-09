using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BankingSystem.Core.Models;
using BankingSystem.Core.Services;
using BankingSystem.Core.Data;
using BankingSystem.WPF.Commands;
using System.Windows.Input;
using BankingSystem.WPF.ViewModels.Base;
using System.Collections.ObjectModel;


namespace BankingSystem.WPF.ViewModels.Customers
{
    public class CustomersViewModel : ViewModelBase
    {
        private readonly CustomerService _customerService;

        public ObservableCollection<Customer> Customers { get; set; }

        // simple fields for adding customer
        private string _name;
        private int _age;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        public ICommand AddCustomerCommand { get; }

        public CustomersViewModel()
        {
            var context = new BankingDbContext();
            var logger = new LoggingService();

            _customerService = new CustomerService(context, logger);

            LoadCustomers();

            AddCustomerCommand = new RelayCommand(_ => AddCustomer());
        }

        private void LoadCustomers()
        {
            Customers = new ObservableCollection<Customer>(
                _customerService.GetAllCustomers()
            );
        }

        private void AddCustomer()
        {
            var customer = new Customer
            {
                Name = Name,
                Age = Age
            };

            _customerService.CreateCustomer(customer);

            Customers.Add(customer);

            Name = "";
            Age = 0;
        }
    }
}