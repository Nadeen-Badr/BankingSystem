using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.WPF;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
namespace BankingSystem.WPF.ViewModels.Customers
{
    public class EditCustomerViewModel : ViewModelBase
    {
        private readonly AppServices _services;

        public EditCustomerViewModel(AppServices services)
        {
            _services = services;

            Genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();

            SaveCommand = new RelayCommand(_ => ExecuteSafely(Save));
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());

            LoadCustomer();
        }

        public List<Gender> Genders { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action RequestClose;

        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        private int _age;
        public int Age { get => _age; set => SetProperty(ref _age, value); }

        private string _address;
        public string Address { get => _address; set => SetProperty(ref _address, value); }

        private Gender _gender;
        public Gender Gender { get => _gender; set => SetProperty(ref _gender, value); }

        private void LoadCustomer()
        {
            var id = AppSession.CurrentCustomerId;
            if (id == null) return;

            var customer = _services.CustomerService.GetCustomerById(id.Value);
            if (customer == null) return;

            Name = customer.Name;
            Age = customer.Age;
            Address = customer.Address;
            Gender = customer.Gender;
        }

        private void Save()
        {
            var id = AppSession.CurrentCustomerId;
            if (id == null) throw new InvalidOperationException();

            _services.CustomerService.UpdateCustomer(new Customer
            {
                Id = id.Value,
                Name = Name,
                Age = Age,
                Address = Address,
                Gender = Gender
            });

            RequestClose?.Invoke();
        }

        private void ExecuteSafely(Action action)
        {
            try { action(); }
            catch (Exception ex) { ErrorHandler.Handle(ex); }
        }
    }
}