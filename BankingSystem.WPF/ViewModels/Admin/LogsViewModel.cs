using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.Commands;
using BankingSystem.WPF.Helpers;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Windows.Input;

namespace BankingSystem.WPF.ViewModels.Admin
{
    public class LogsViewModel : ViewModelBase
    {
        private readonly ILoggerService _logger;

        private string _logs;
        public string Logs
        {
            get => _logs;
            set => SetProperty(ref _logs, value);
        }

        public ICommand RefreshLogsCommand { get; }

        public LogsViewModel(ILoggerService logger)
        {
            _logger = logger;

            RefreshLogsCommand = new RelayCommand(_ => ExecuteSafely(LoadLogs));

            LoadLogs();
        }

        private void LoadLogs()
        {
            Logs = _logger.ReadLogs();
        }

        private void ExecuteSafely(Action action)
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
    }
}