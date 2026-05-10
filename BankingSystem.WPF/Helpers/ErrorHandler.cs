using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Windows;
using BankingSystem.Core.Exceptions;

namespace BankingSystem.WPF.Helpers
{
    public static class ErrorHandler
    {
        public static void Handle(Exception ex)
        {
            if (ex is BusinessException)
            {
                MessageBox.Show(
                    ex.Message,
                    "Business Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show(
                    "Unexpected system error occurred",
                    "System Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}