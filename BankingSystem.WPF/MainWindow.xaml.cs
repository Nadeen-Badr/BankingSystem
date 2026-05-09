using BankingSystem.WPF.Commands;
using BankingSystem.WPF.ViewModels;
using BankingSystem.WPF.ViewModels.Accounts;
using BankingSystem.WPF.ViewModels.Base;
using System.Windows.Input;

namespace BankingSystem.WPF
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}