using BankingSystem.Core.Data;
using BankingSystem.Core.Exceptions;
using BankingSystem.Core.Models;
using BankingSystem.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

public class CustomerService : ICustomerService
{
    private readonly BankingDbContext _context;
    private readonly ILoggerService _logger;

    public CustomerService(BankingDbContext context, ILoggerService logger)
    {
        _context = context;
        _logger = logger;
    }

    // ================= CREATE =================
    public Customer CreateCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();

        _logger.Log($"CREATE_CUSTOMER_SUCCESS | ID:{customer.Id} | Name:{customer.Name}");

        return customer;
    }

    // ================= UPDATE =================
    public void UpdateCustomer(Customer customer)
    {
        var existing = _context.Customers.Find(customer.Id);

        if (existing == null)
        {
            _logger.Log($"UPDATE_CUSTOMER_FAILED | ID:{customer.Id} | Reason: Not found");
            throw new CustomerNotFoundException(customer.Id);
        }

        existing.Name = customer.Name;
        existing.Age = customer.Age;
        existing.Gender = customer.Gender;
        existing.Address = customer.Address;

        _context.SaveChanges();

        _logger.Log($"UPDATE_CUSTOMER_SUCCESS | ID:{customer.Id} | Name:{customer.Name}");
    }

    // ================= DELETE =================
    public void CloseCustomer(int customerId)
    {
        var customer = _context.Customers.Find(customerId);

        if (customer == null)
        {
            _logger.Log($"CLOSE_CUSTOMER_FAILED | ID:{customerId} | Reason: Not found");
            throw new CustomerNotFoundException(customerId);
        }

        _context.Customers.Remove(customer);
        _context.SaveChanges();

        _logger.Log($"CLOSE_CUSTOMER_SUCCESS | ID:{customerId} | Name:{customer.Name}");
    }


    // ================= READ =================
    public List<Customer> GetAllCustomers()
    {
        return _context.Customers.ToList();
    }
    public Customer GetCustomerById(int id)
    {
        try
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);

            if (customer == null)
            {
                _logger.Log($"GET_CUSTOMER_FAILED | ID:{id} | Reason: Not found");
                return null;
            }

            _logger.Log($"GET_CUSTOMER_SUCCESS | ID:{id}");

            return customer;
        }
        catch (Exception ex)
        {
            _logger.Log($"GET_CUSTOMER_ERROR | ID:{id} | Exception:{ex.Message}");
            throw;
        }
    }
}