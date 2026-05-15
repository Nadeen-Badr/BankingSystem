using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
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
        private readonly AppServices _services;

        public ServicesViewModel(AppServices services)
        {
            _services = services;

            Certificates = new ObservableCollection<Certificate>();

            LoadCommand = new RelayCommand(_ => ExecuteSafely(Load));
            BuyCertificateCommand = new RelayCommand(_ => ExecuteSafely(BuyCertificate));
            UpdateCertificateCommand = new RelayCommand(_ => ExecuteSafely(UpdateCertificate));
            DeleteCertificateCommand = new RelayCommand(_ => ExecuteSafely(DeleteCertificate));

            CreateCardCommand = new RelayCommand(_ => ExecuteSafely(CreateCard));
            UpdateLimitCommand = new RelayCommand(_ => ExecuteSafely(UpdateLimit));

            Load();
        }

        // ================= DATA =================
        public ObservableCollection<Certificate> Certificates { get; }

        private Certificate _selectedCertificate;
        public Certificate SelectedCertificate
        {
            get => _selectedCertificate;
            set => SetProperty(ref _selectedCertificate, value);
        }

        private decimal _certificatePrice;
        public decimal CertificatePrice
        {
            get => _certificatePrice;
            set => SetProperty(ref _certificatePrice, value);
        }

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

        private decimal _creditLimit;
        public decimal CreditLimit
        {
            get => _creditLimit;
            set => SetProperty(ref _creditLimit, value);
        }

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

        public string CardInfo =>
            Card == null
                ? "No credit card found for this customer."
                : $"Limit: {Card.CashLimit:C} | Expires: {Card.ExpiryDate:MM/yyyy}";

        // ================= COMMANDS =================
        public ICommand LoadCommand { get; }
        public ICommand BuyCertificateCommand { get; }
        public ICommand UpdateCertificateCommand { get; }
        public ICommand DeleteCertificateCommand { get; }
        public ICommand CreateCardCommand { get; }
        public ICommand UpdateLimitCommand { get; }

        // ================= LOAD =================
        private void Load()
        {
            try
            {
                var customerId = AppSession.CurrentCustomerId;
                if (customerId == null) return;

                Certificates.Clear();

                var certs = _services.CertificateService.GetByCustomer(customerId.Value);
                foreach (var c in certs)
                    Certificates.Add(c);

                Card = _services.CreditCardService.GetByCustomer(customerId.Value);
                OnPropertyChanged(nameof(CardInfo));
            }
            catch (Exception ex)
            {
                ErrorHandler.Handle(ex);
            }
        }

        // ================= ACTIONS =================
        private void BuyCertificate()
        {
            var id = AppSession.CurrentCustomerId;
            if (id == null) return;

            _services.CertificateService.BuyCertificate(id.Value, CertificatePrice, SelectedPeriod);
            Load();
        }

        private void UpdateCertificate()
        {
            if (SelectedCertificate == null) return;

            _services.CertificateService.UpdateCertificate(
                SelectedCertificate.Id,
                CertificatePrice,
                SelectedPeriod);
         
        
        Load();
          
        }

        private void DeleteCertificate()
        {
            if (SelectedCertificate == null) return;

            _services.CertificateService.DeleteCertificate(SelectedCertificate.Id);
            Load();
        }

        private void CreateCard()
        {
            var id = AppSession.CurrentCustomerId;
            if (id == null) return;

            _services.CreditCardService.CreateCard(id.Value, CreditLimit);
            Load();
        }

        private void UpdateLimit()
        {
            var id = AppSession.CurrentCustomerId;
            if (id == null) return;

            _services.CreditCardService.UpdateLimit(id.Value, CreditLimit);
            Load();
        }

        // ================= SAFE =================
        private void ExecuteSafely(Action action)
        {
            try { action(); }
            catch (Exception ex) { ErrorHandler.Handle(ex); }
        }
    }
}