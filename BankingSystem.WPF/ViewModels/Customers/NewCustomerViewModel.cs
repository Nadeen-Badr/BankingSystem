using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.WPF;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Windows.Input;
namespace BankingSystem.WPF.ViewModels.Customers
{
    public class NewCustomerViewModel : ViewModelBase
    {
        private readonly AppServices _services;

        public NewCustomerViewModel(AppServices services)
        {
            _services = services;

            CreateCustomerCommand = new RelayCommand(_ => ExecuteSafely(CreateCustomer), _ => CanCreate());
            BackCommand = new RelayCommand(_ => RequestBackToLogin?.Invoke());

            Gender = Gender.Male;
        }

        public ICommand CreateCustomerCommand { get; }
        public ICommand BackCommand { get; }

        public event Action RequestBackToLogin;
        public Array Genders => Enum.GetValues(typeof(Gender));
        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        private int _age;
        public int Age { get => _age; set => SetProperty(ref _age, value); }

        private string _address;
        public string Address { get => _address; set => SetProperty(ref _address, value); }

        private Gender _gender;
        public Gender Gender { get => _gender; set => SetProperty(ref _gender, value); }

        private string _message;
        public string Message { get => _message; set => SetProperty(ref _message, value); }

        private bool _isSuccess;
        public bool IsSuccess { get => _isSuccess; set => SetProperty(ref _isSuccess, value); }

        private void CreateCustomer()
        {
            _services.CustomerService.CreateCustomer(new Customer
            {
                Name = Name,
                Age = Age,
                Gender = Gender,
                Address = Address
            });

            _services.Logger.Log($"NEW_CUSTOMER | {Name}");

            IsSuccess = true;
            Message = "Created successfully";

            Name = "";
            Age = 0;
            Address = "";
            Gender = Gender.Male;
        }

        private bool CanCreate()
            => !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(Address)
            && Age > 18;

        private void ExecuteSafely(Action action)
        {
            try { action(); }
            catch (Exception ex)
            {
                IsSuccess = false;
                Message = ex.Message;
                ErrorHandler.Handle(ex);
            }
        }
    }
}