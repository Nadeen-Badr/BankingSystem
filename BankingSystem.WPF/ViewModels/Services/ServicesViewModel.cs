using BankingSystem.Core.Data;
using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BankingSystem.WPF.ViewModels.Services
{
    public class ServicesViewModel : ViewModelBase
    {
        private readonly ICertificateService _certificateService;
        private readonly ICreditCardService _creditCardService;
        private readonly BankingDbContext _context;

        public ObservableCollection<Certificate> Certificates { get; set; }

        private Certificate _selectedCertificate;
        public Certificate SelectedCertificate
        {
            get => _selectedCertificate;
            set => SetProperty(ref _selectedCertificate, value);
        }

        // Inputs
        public decimal CertificatePrice { get; set; }

        public List<CertificatePeriod> Periods { get; } =
            Enum.GetValues(typeof(CertificatePeriod))
                .Cast<CertificatePeriod>()
                .ToList();

        private CertificatePeriod _selectedPeriod;
        public CertificatePeriod SelectedPeriod
        {
            get => _selectedPeriod;
            set => SetProperty(ref _selectedPeriod, value);
        }

        public decimal CreditLimit { get; set; }

        private CreditCard _card;
        public CreditCard Card
        {
            get => _card;
            set
            {
                if (SetProperty(ref _card, value))
                    OnPropertyChanged(nameof(CardInfo));
            }
        }

        public string CardInfo
        {
            get
            {
                if (Card == null)
                    return "No credit card found for this customer.";

                return $"Limit: {Card.CashLimit:C} | Expires: {Card.ExpiryDate:MM/yyyy}";
            }
        }

        // Commands
        public ICommand LoadCommand { get; }
        public ICommand BuyCertificateCommand { get; }
        public ICommand UpdateCertificateCommand { get; }
        public ICommand DeleteCertificateCommand { get; }

        public ICommand CreateCardCommand { get; }
        public ICommand UpdateLimitCommand { get; }

        public ServicesViewModel(
            ICertificateService certificateService,
            ICreditCardService creditCardService,
            BankingDbContext context)
        {
            _certificateService = certificateService;
            _creditCardService = creditCardService;
            _context = context;

            Certificates = new ObservableCollection<Certificate>();

            LoadCommand = new RelayCommand(_ => SafeExecute(Load));
            BuyCertificateCommand = new RelayCommand(_ => SafeExecute(BuyCertificate));
            UpdateCertificateCommand = new RelayCommand(_ => SafeExecute(UpdateCertificate));
            DeleteCertificateCommand = new RelayCommand(_ => SafeExecute(DeleteCertificate));

            CreateCardCommand = new RelayCommand(_ => SafeExecute(CreateCard));
            UpdateLimitCommand = new RelayCommand(_ => SafeExecute(UpdateLimit));

            Load();
        }

        // ================= SAFE WRAPPER =================
        private void SafeExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex);
            }
        }

        // ================= LOAD =================
        private void Load()
        {
            var customerId = AppSession.CurrentCustomerId;
            if (customerId == null) return;

            Certificates.Clear();

            var certs = _context.Certificates
                .Where(c => c.CustomerId == customerId)
                .ToList();

            foreach (var c in certs)
                Certificates.Add(c);

            Card = _creditCardService.GetByCustomer(customerId.Value);
        }

        // ================= CERTIFICATES =================
        private void BuyCertificate()
        {
            if (AppSession.CurrentCustomerId == null) return;

            _certificateService.BuyCertificate(
                AppSession.CurrentCustomerId.Value,
                CertificatePrice,
                SelectedPeriod);

            Load();
        }

        private void UpdateCertificate()
        {
            if (SelectedCertificate == null) return;

            _certificateService.UpdateCertificate(
                SelectedCertificate.Id,
                CertificatePrice,
                SelectedPeriod);

            Load();
        }

        private void DeleteCertificate()
        {
            if (SelectedCertificate == null) return;

            _certificateService.DeleteCertificate(SelectedCertificate.Id);

            Load();
        }

        // ================= CREDIT CARD =================
        private void CreateCard()
        {
            if (AppSession.CurrentCustomerId == null) return;

            _creditCardService.CreateCard(
                AppSession.CurrentCustomerId.Value,
                CreditLimit);

            Load();
        }

        private void UpdateLimit()
        {
            if (AppSession.CurrentCustomerId == null) return;

            _creditCardService.UpdateLimit(
                AppSession.CurrentCustomerId.Value,
                CreditLimit);

            Load();
        }
    }
}