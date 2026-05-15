using BankingSystem.Core.Enums;
using System;

namespace BankingSystem.WPF.Helpers
{
    /// <summary>
    /// Stores lightweight session information for the currently logged-in user.
    /// Avoid storing EF entities here to prevent UI/EF coupling.
    /// </summary>
    public static class AppSession
    {
        public static UserRole? CurrentRole { get; private set; }

        public static int? CurrentCustomerId { get; private set; }

        public static string CurrentCustomerName { get; private set; }

        /// <summary>
        /// Triggered when authentication/session state changes.
        /// Used by UI components that react to role updates.
        /// </summary>
        public static event Action RoleChanged;

        public static void SetRole(UserRole role)
        {
            CurrentRole = role;
            RoleChanged?.Invoke();
        }

        public static void SetCustomer(
            int customerId,
            string customerName)
        {
            
            CurrentCustomerId = customerId;
            CurrentCustomerName = customerName;
            CurrentRole = UserRole.Customer;

            RoleChanged?.Invoke();
        }

        public static void Clear()
        {
            CurrentRole = null;
            CurrentCustomerId = null;
            CurrentCustomerName = null;

            RoleChanged?.Invoke();
        }
    }
}