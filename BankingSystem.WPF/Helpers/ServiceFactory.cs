using BankingSystem.Core.Data;
using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;

namespace BankingSystem.WPF.Helpers
{
    /// <summary>
    /// Central place for creating shared services and DbContext instances.
    /// Prevents duplication and multiple DbContext issues.
    /// </summary>
    public static class ServiceFactory
    {
        public static BankingDbContext CreateContext()
            => new BankingDbContext();

        public static ILoggerService CreateLogger()
            => new LoggingService();

        public static ICustomerService CreateCustomerService()
            => new CustomerService(CreateContext(), CreateLogger());
    }
}