using BankingSystem.Core.Data;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
public class CustomerService : ICustomerService
{
    private readonly BankingDbContext _context;
    private readonly ILoggerService _logger;

    public CustomerService(BankingDbContext context, ILoggerService logger)
    {
        _context = context;
        _logger = logger;
    }

    public Customer CreateCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();

        _logger.Log($"CREATE_CUSTOMER | ID:{customer.Id} | Name:{customer.Name}");

        return customer;
    }

    public void UpdateCustomer(Customer customer)
    {
        var existing = _context.Customers.Find(customer.Id);

        if (existing == null)
            throw new Exception("Customer not found");

        existing.Name = customer.Name;
        existing.Age = customer.Age;
        existing.Gender = customer.Gender;
        existing.Address = customer.Address;

        _context.SaveChanges();

        _logger.Log($"UPDATE_CUSTOMER | ID:{customer.Id}");
    }

    public void CloseCustomer(int customerId)
    {
        var customer = _context.Customers.Find(customerId);

        if (customer == null)
            throw new Exception("Customer not found");

        _context.Customers.Remove(customer);
        _context.SaveChanges();

        _logger.Log($"CLOSE_CUSTOMER | ID:{customerId}");
    }

    public List<Customer> GetAllCustomers()
    {
        return _context.Customers.ToList();
    }
}