
using System.Collections.Generic;
using BankingSystem.Core.Services.Interfaces;


    public class FakeLogger : ILoggerService
    {
        public List<string> Logs { get; } = new List<string>();

        public void Log(string message)
        {
            Logs.Add(message);
        }

        public string ReadLogs()
        {
            return string.Join("\n", Logs);
        }
    }

