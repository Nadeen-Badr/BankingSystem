using System.Windows.Controls;
using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.ViewModels.Admin;

namespace BankingSystem.WPF.Views.Admin
{
    /// <summary>
    /// Code-behind is used ONLY to inject ViewModel manually.
    /// No UI logic should exist here.
    /// </summary>
    public partial class LogsView : UserControl
    {
        public LogsView()
        {
            InitializeComponent();

            // Manual lightweight dependency creation (no DI container used)
            ILoggerService logger = new LoggingService();

            DataContext = new LogsViewModel(logger);
        }
    }
}