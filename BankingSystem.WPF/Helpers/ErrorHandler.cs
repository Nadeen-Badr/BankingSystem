using System;
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
                // Expected business rule violations (e.g. insufficient balance, invalid operation)
                MessageBox.Show(
                    ex.Message,
                    "Business Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Unexpected system errors (DB failure, null reference, etc.)
            MessageBox.Show(
                "Unexpected system error occurred",
                "System Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}