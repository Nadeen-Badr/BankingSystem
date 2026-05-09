using BankingSystem.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Services
{
    public class LoggingService : ILoggerService
    {
        private static readonly string FilePath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "BankingSystem",
                "bank-log.txt"
            );

        public void Log(string message)
        {
            var directory = Path.GetDirectoryName(FilePath);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {message}";
            File.AppendAllText(FilePath, log + Environment.NewLine);
        }

        public string ReadLogs()
        {
            if (!File.Exists(FilePath))
                return "No logs found.";

            return File.ReadAllText(FilePath);
        }
    }
}
