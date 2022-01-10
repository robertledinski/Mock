using DatabaseAbstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseInfrastructure
{
    public class ApplicationContext : MockContext
    {
        public ApplicationContext(DbContextOptions<MockContext> options) : base(options)
        {

        }

    }

    public static class ModelBuilderHelper
    {
        /// <summary>
        /// Helper
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static EntityTypeBuilder<TEntity> Required<TEntity>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, object>> expression) where TEntity : class
        {
            builder.Property(expression).IsRequired();
            return builder;
        }
    }

    public class MockContext : DbContext, IDbContext
    {
        public MockContext(DbContextOptions<MockContext> options)
        : base(options)
        {
        }

        

        public string GetDbName()
          => Database.ProviderName;

        public Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker GetChangeTracker()
          => this.ChangeTracker;

        public Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction CurrentTransaction()
            => Database.CurrentTransaction;

        public Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction BeginTransaction()
            => Database.BeginTransaction();

        public async Task<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
         => await Database.BeginTransactionAsync(cancellationToken);

        public void RollBackTransaction()
            => Database.RollbackTransaction();

        public async Task RollBackTransactionAsync(CancellationToken cancellationToken)
          => await Database.RollbackTransactionAsync(cancellationToken);

        public void CommitTransaction()
            => Database.CommitTransaction();

        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
            => await Database.CommitTransactionAsync(cancellationToken);

        

        public DbSet<User> Users { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyConversion> CurrencyConversions { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<StandingOrder> StandingOrders { get; set; }
        public DbSet<DirectDebit> DirectDebits { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        /// Add tables for SSO (if time allows)

        #region Required
        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            mb.Entity<User>(u =>
            {
                u.Required(x => x.FistName)
                 .Required(x => x.LastName)
                 .Required(x => x.FailedLoginRetryCount)
                 .Required(x => x.Locked)
                 .Required(x => x.AddressId)
                 .Required(x => x.Password)
                 .HasKey(x => x.Id);

                u.HasIndex(x => x.Email).IsUnique();     // Test if required will be applied
                u.HasIndex(x => x.UserName).IsUnique();  // Test if required will be applied

                // In case there is only one address per user
                u.HasOne(x => x.Address)
                 .WithOne(x => x.User)
                 .HasForeignKey<Address>(x => x.Id);

                u.HasMany(x => x.Accounts)
                 .WithOne(x => x.User)
                 .HasForeignKey(x => x.Id);
            });

            mb.Entity<Bank>(b =>
            {
                b.Required(x => x.Name)
                 .HasKey(x => x.Id);

                b.HasMany(x => x.Accounts)
                 .WithOne(x => x.Bank)
                 .HasForeignKey(x => x.BankId);
            });

            mb.Entity<Currency>(c =>
            {
                c.Required(x => x.Name)
                 .HasKey(x => x.Id);
            });

            mb.Entity<CurrencyConversion>(cc =>
            {
                cc.Required(x => x.SourceCurrencyId)
                 .Required(x => x.CurrencyToConvertToId)
                 .Required(x => x.ConversionValue)
                 .HasKey(x => x.Id);

                cc.HasOne(x => x.SourceCurrency)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

                cc.HasOne(x => x.CurrencyToConvertTo)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            });

            mb.Entity<Account>(a =>
            {
                a.Required(x => x.AvailableBalance)
                 .Required(x => x.RedMinus)
                 .Required(x => x.AvailableMinus)
                 .Required(x => x.BankId)
                 .Required(x => x.AccountNumber)
                 .Required(x => x.AccountId)
                 .Required(x => x.CurrentBalance)
                 .Required(x => x.DailyWithdrawalLimit)
                 .Required(x => x.Name)
                 .Required(x => x.Type)
                 .Required(x => x.SubType)
                 .Required(x => x.Locked)
                 .Required(x => x.UserId)
                 .Required(x => x.CurrencyId)
                 .Required(x => x.CreditCardId)
                 .Required(x => x.BankId)
                 .HasKey(x => x.Id);

                a.Property(x => x.SortCode);

                a.HasOne(x => x.User)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.UserId);

                a.HasOne(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId);

                a.HasOne(x => x.CreditCard)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.CreditCardId);

                a.HasOne(x => x.Bank)
                .WithMany(x => x.Accounts)
                .HasForeignKey(x => x.BankId);
            });

            mb.Entity<Party>(p => 
            {
                p.HasKey(x => x.Id);
            });

            mb.Entity<StandingOrder>(so =>
            {
                so.HasKey(x => x.Id);
            });

            mb.Entity<DirectDebit>(dd =>
            {
                dd.HasKey(x => x.Id);
            });

            mb.Entity<CreditCard>(cc =>
            {
                cc.Required(x => x.TypeId)
                 .Required(x => x.Name)
                 .HasKey(x => x.Id);

                cc.HasMany(x => x.Accounts)
                .WithOne(x => x.CreditCard)
                .HasForeignKey(x => x.CreditCardId);
            });

            mb.Entity<Address>(a =>
            {
                a.Required(x => x.Street)
                 .Required(x => x.City)
                 .Required(x => x.CountryId)
                 .Required(x => x.Zip)
                 .HasKey(x => x.Id);

                a.HasOne(x => x.User)
                .WithOne(x => x.Address)
                .HasForeignKey<User>(x => x.AddressId);

                a.HasOne(x => x.Country)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.CountryId);
            });

            mb.Entity<Country>(c => 
            {
                c.Required(x => x.Name)
                .HasKey(x => x.Id);

                c.HasMany(x => x.Addresses)
                .WithOne(x => x.Country)
                .HasForeignKey(x => x.CountryId);
            });

            mb.Entity<Transaction>(t => 
            {
                t.Required(x => x.Status)
                .Required(x => x.Amount)
                .Required(x => x.CardType)
               // .Required(x => x.CurrencyId)
                .Required(x => x.Date).HasKey(x => x.Id);


                t.Property(x => x.Description).IsRequired().HasMaxLength(100);

                t.Property(x => x.MerchantDetails).HasMaxLength(100);

                t.HasOne(x => x.Account)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.AccountId);

             /*   t.HasOne(x => x.Currency)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); */
            });

        }
        #endregion

    }



    public class User : PrimaryKey, IPrimaryKey
    {
        public User()
        {
            Locked = false;
            FailedLoginRetryCount = 0;
        }

        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public Int16 FailedLoginRetryCount { get; set; }
        public bool Locked { get; set; }


        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual List<Account> Accounts { get; } = new();

    }

    public class Bank : PrimaryKey, IPrimaryKey
    {
        public string Name { get; set; }

        public virtual List<Account> Accounts { get; set; } = new();
    }


    public class Currency : PrimaryKey, IPrimaryKey
    {
        public string Name { get; set; }
    }

    public class CurrencyConversion : PrimaryKey, IPrimaryKey
    {
        /// <summary>
        /// This should be updated daily based on the market
        /// </summary>
        public decimal ConversionValue { get; set; }

        
        public int SourceCurrencyId { get; set; }
        public int CurrencyToConvertToId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("SourceCurrencyId")]
        public virtual Currency SourceCurrency { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("CurrencyToConvertToId")]
        public virtual Currency CurrencyToConvertTo { get; set; }
    }

    public class Account : PrimaryKey, IPrimaryKey
    {

        public string Name { get; set; }
        /// <summary>
        /// Values here will change on each credit
        /// by converting values which are being paid to 
        /// current currency of the account
        /// </summary>
        public decimal CurrentBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal AvailableMinus { get; set; }
        public decimal RedMinus { get; set; }
        public decimal DailyWithdrawalLimit { get; set; }
        public AccountType Type { get; set; }
        public AccountSubType SubType { get; set; }
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountId { get; set; }
        public string SecondaryIdentification { get; set; }
        public int Locked { get; set; }


        public int UserId { get; set; } 
        /// <summary>
        /// Here the assumpsution is that every Credit card is tied 
        /// to one account which is the case for clients in Croatia
        /// unless you are coropration, then I think this changes.
        /// Because this is test case I will keep it simple and will not 
        /// cover that edge case.
        /// </summary>
        public int CreditCardId { get; set; }
        public int CurrencyId { get; set; }
        public int BankId { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual User User { get; set; }
        public virtual CreditCard CreditCard { get; set; }
        public virtual List<Transaction> Transactions { get; set; }
    }

    /// <summary>
    /// Created for parties in Account.json
    /// </summary>
    public class Party : PrimaryKey, IPrimaryKey { }

    /// <summary>
    /// Created for standing order in Account.json
    /// </summary>
    public class StandingOrder : PrimaryKey, IPrimaryKey { }

    /// <summary>
    /// Created for direct debits in Account.json. It wouldn't be a table
    /// if I had more information about it.
    /// </summary>
    public class DirectDebit : PrimaryKey, IPrimaryKey { }

    /// <summary>
    /// No need for a table because it never changes only expands,
    /// but can exist
    /// </summary>
    public enum AccountType : short
    {
        Unknown = 0,
        Personal = 1,
        Business = 2
    }

    public enum AccountSubType : short
    {
        Unknown = 0,
        CurrentAccount = 1
    }

    public enum CurrencyType : short
    { 
        Unknown = 0,
        GBP = 1,
        EUR = 2
        // add more if needed
    }

    public class CreditCard : PrimaryKey, IPrimaryKey
    {
        public string Name { get; set; }

        /// <summary>
        /// Because CC types aren't changing often we can define them
        /// in DB table and then have hardcoded Ids in database
        /// </summary>
        public CreditCardType TypeId { get; set; }


        public virtual List<Account> Accounts { get; set; }
    }



    /// <summary>
    /// Because CC types aren't changing often we can define them
    /// in DB table and then have hardcoded Ids in database
    /// </summary>
    public enum CreditCardType : short
    {
        Debit = 1,
        Credit = 2,
        DeferredPayment = 3,
        Prepaid = 4,
        Virutal = 5,
        Aqua = 6
    }


    public class Address : PrimaryKey, IPrimaryKey
    {
        public int CountryId { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Street { get; set; }

        public virtual Country Country { get; set; }
        public virtual User User { get; set; }
    }

    public class Country : PrimaryKey, IPrimaryKey
    {
        public string Name { get; set; }

        public virtual List<Address> Addresses { get; set; }
    }

    /// <summary>
    /// Primary key should be Guid because there can 
    /// be very big number of transactions
    /// </summary>
    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string MerchantDetails { get; set; }
        public CreditCardType CardType { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }

        // Foreign keys
        public int AccountId { get; set; }


        public virtual Account Account { get; set; }
    }

    /// <summary>
    /// We should create table for statuses because we can have custom logic based
    /// on any status, so enumeration might not be good solution, however,
    /// because this is test I will use enumeration
    /// Example would be workflows, task service recalculations, etc.
    /// </summary>
    public enum Status
    {
        Booked = 1
    }
}
