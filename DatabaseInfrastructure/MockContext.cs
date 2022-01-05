using DatabaseAbstructions;
using DatabaseInfrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DatabaseInfrastructure
{
    public class ApplicationContext : MockContext { }

    public class MockContext : DbContext, IDbContext
    {
    }

    public class User : PrimaryKey, IPrimaryKey
    {
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public Int16 FailedLoginRetryCount { get; set; }
        public bool Locked { get; set; }
        

        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual List<BankAccount> BankAccounts { get; } = new();
    }

    public class BankAccount : PrimaryKey, IPrimaryKey
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public bool Locked { get; set; }

        public virtual User User { get; set; }
        public virtual List<Account> Accounts { get; } = new();
    }

    public class Account : PrimaryKey, IPrimaryKey
    {
        public int UserId { get; set; }
        public double AvailableLimit { get; set; }
        public double RedLimit { get; set; }
        public double Balance { get; set; }
        public int Locked { get; set; }
        public int CreditCardId { get; set; }

        public virtual CreditCard CreditCard { get; set; }
    }

    public class CreditCard : PrimaryKey, IPrimaryKey
    {
        public string Name { get; set; }
        public double AvailableLimit { get; set; }
        public double RedLimit { get; set; }
    }

    public class Address : PrimaryKey, IPrimaryKey
    {
        public int CountryId { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Street { get; set; }

        public virtual Country Country { get; set; }
    }

    public class Country : PrimaryKey, IPrimaryKey
    {
        public string Name { get; set; }
    }

    public class Transaction : PrimaryKey, IPrimaryKey
    {
        public int UserId { get; set; }
        public int CurrencyId { get; set; }
    }
}
