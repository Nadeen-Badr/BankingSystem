using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public LogsViewModel()
        {
            _logger = new LoggingService(); // simple manual DI
            Logs = _logger.ReadLogs();
        }
    }
}
