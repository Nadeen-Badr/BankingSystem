using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankingSystem.WPF.ViewModels.Customers
{
    public class EditCustomerViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;

        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public Gender Gender { get; set; }

        public List<Gender> Genders { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action RequestClose;

        public EditCustomerViewModel(ICustomerService customerService)
        {
            _customerService = customerService;

            Genders = Enum.GetValues(typeof(Gender))
                          .Cast<Gender>()
                          .ToList();

            LoadCustomer();

            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }

        private void LoadCustomer()
        {
            var customer = _customerService
                .GetAllCustomers()
                .FirstOrDefault(c => c.Id == AppSession.CurrentCustomerId);

            if (customer == null)
                return;

            Name = customer.Name;
            Age = customer.Age;
            Address = customer.Address;
            Gender = customer.Gender;

            OnPropertyChanged(string.Empty);
        }

        private void Save()
        {
            var updatedCustomer = new Customer
            {
                Id = AppSession.CurrentCustomerId.Value,
                Name = Name,
                Age = Age,
                Address = Address,
                Gender = Gender
            };

            _customerService.UpdateCustomer(updatedCustomer);

            RequestClose?.Invoke();
        }
    }
}
