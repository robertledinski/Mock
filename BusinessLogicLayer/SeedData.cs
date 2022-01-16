using DatabaseInfrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            // TODO: Write it using UOW and IRepository pattern
            using (var context = new MockContext(serviceProvider.GetRequiredService<DbContextOptions<MockContext>>()))
            {
                try
                {
                    // Create db first execution
                    context.Database.EnsureCreated();
                    // DB Location after creation "(localdb)\MSSQLLocalDB" - navigate with SSMS

                    // Check if there is seed data
                    if (context.Banks.Any())
                        return;   // DB has been seeded

                    var userAndResult = SeedUserCountryAndAddress(context);
                    if (!userAndResult.Item2)
                        return;

                    // Parse Account.json and seed initial data 
                    SeedDataFromApplicationJson(context, userAndResult.Item1);

                    // Commit to db
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private static Tuple<User, bool> SeedUserCountryAndAddress(MockContext context)
        {
            var user = new User();

            if (context.Users.Any())
                return new Tuple<User, bool>(user, false);

            var country = new Country()
            {
                Name = "Test Country"
            };

            var address = new Address()
            {
                City = "City",
                Street = "Street",
                Zip = "10000",
                Country = country
            };

            user = new User
            {
                Address = address,
                FistName = "John",
                LastName = "Smith",
                Email = "john@smith.com",
                Password = CryptoHelper.GetStringSha256Hash("Test"),
                UserName = "john@smith.com"
            };

            context.Users.Add(user);

            return new Tuple<User, bool>(user, true);
        }

        private static DataSourceInput LoadJson()
        {
            var dataSourceInput = new DataSourceInput();
            using (StreamReader r = new StreamReader("Account.json"))
            {
                string json = r.ReadToEnd();
                dataSourceInput = JsonConvert.DeserializeObject<DataSourceInput>(json);
            }

            return dataSourceInput;
        }

        private static void SeedDataFromApplicationJson(MockContext context, User user)
        {
            if (user == null)
                return;

            // Read to account.json and parse it
            var accountInput = LoadJson();


            var creditCards = new List<CreditCard>() 
            {
                new CreditCard() { Name = "Test", TypeId = CreditCardType.Debit// Id = 1
                },
                new CreditCard() { Name = "Test", TypeId = CreditCardType.Credit//, Id = 2 
                }
            };


            context.CreditCards.AddRange(creditCards);

            context.Currencies.Add(new Currency() { Name = "GBP"}); // Id = 1

            context.Banks.Add(new Bank()
            {
                Name = accountInput.BankName,
                Accounts = accountInput.Accounts.Select(x => x.ConvertToAccountDTO(user)).ToList()
            });
        }
    }
}
