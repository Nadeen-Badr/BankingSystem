using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BankingSystem.Core.Models;

namespace BankingSystem.Core.Services.Interfaces
{
    public interface ICreditCardService
    {
        CreditCard CreateCard(int customerId, decimal cashLimit);

        void UpdateLimit(int customerId, decimal newLimit);

        CreditCard GetByCustomer(int customerId);
    }
}