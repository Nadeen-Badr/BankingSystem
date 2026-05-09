using BankingSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Core.Services.Interfaces
{
    public interface ICustomerService
    {
        Customer CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void CloseCustomer(int customerId);
        List<Customer> GetAllCustomers();
    }
}
