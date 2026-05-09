using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Windows.Input;

namespace BankingSystem.WPF.ViewModels.Customers
{
    public class NewCustomerViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILoggerService _logger;

        public NewCustomerViewModel(ICustomerService customerService, ILoggerService logger)
        {
            _customerService = customerService;
            _logger = logger;

            CreateCustomerCommand = new RelayCommand(_ => CreateCustomer());
            BackCommand = new RelayCommand(_ => RequestBackToLogin?.Invoke());
        }

        // ================= INPUTS =================
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int _age;
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        private Gender _gender;
        public Gender Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        // ================= UX MESSAGE =================
        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private bool _isSuccess;
        public bool IsSuccess
        {
            get => _isSuccess;
            set => SetProperty(ref _isSuccess, value);
        }

        // ================= COMMANDS =================
        public ICommand CreateCustomerCommand { get; }
        public ICommand BackCommand { get; }

        public event Action RequestBackToLogin;

        // ================= CORE LOGIC =================
        private void CreateCustomer()
        {
            try
            {
                var customer = new Customer
                {
                    Name = Name,
                    Age = Age,
                    Gender = Gender,
                    Address = Address
                };

                _customerService.CreateCustomer(customer);

                _logger.Log($"NEW_CUSTOMER_CREATED | Name:{Name}");

                // ✅ SUCCESS UX
                IsSuccess = true;
                Message = "Customer created successfully!";

                ResetForm();
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                Message = $"Error: {ex.Message}";
            }
        }

        // ================= RESET FORM =================
        private void ResetForm()
        {
            Name = string.Empty;
            Age = 0;
            Address = string.Empty;
            Gender = Gender.Male;
        }
    }
}