using BankingSystem.Core.Enums;
using BankingSystem.Core.Models;
using System;
public static class AppSession
{
    public static UserRole? CurrentRole { get; private set; }

    public static Customer CurrentCustomer { get; private set; }

    public static event Action RoleChanged;

    public static void SetRole(UserRole role)
    {
        CurrentRole = role;
        RoleChanged?.Invoke();
    }

    public static void SetCustomer(Customer customer)
    {
        CurrentCustomer = customer;
    }

    public static void Clear()
    {
        CurrentRole = null;
        CurrentCustomer = null;
        RoleChanged?.Invoke();
    }
}