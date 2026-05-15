using BankingSystem.Core.Data;
using BankingSystem.Core.Services;
using BankingSystem.Core.Services.Interfaces;
using BankingSystem.WPF.Helpers;

namespace BankingSystem.WPF
{
    public class AppServices
    {
        public BankingDbContext Context { get; }
        public ILoggerService Logger { get; }

        public ICustomerService CustomerService { get; }
        public IAccountService AccountService { get; }
        public ICertificateService CertificateService { get; }
        public ICreditCardService CreditCardService { get; }
        public IReportService ReportService { get; }
        public IPdfExportService PdfExportService { get; }

        public AppServices()
        {
            Context = new BankingDbContext();
            Logger = new LoggingService();

            CustomerService = new CustomerService(Context, Logger);
            AccountService = new AccountService(Context, Logger);
            CertificateService = new CertificateService(Context, Logger);
            CreditCardService = new CreditCardService(Context, Logger);

            ReportService = new ReportService(Context);
            PdfExportService = new PdfExportService();
        }
    }
}