using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInfrastructure
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MockContext(serviceProvider.GetRequiredService<DbContextOptions<MockContext>>()))
            {
                try
                {
                    // Create db first execution
                    context.Database.EnsureCreated();

                    // Check if there is seed data
                    if (context.Banks.Any())
                        return;   // DB has been seeded

                }
                catch (Exception ex)
                {

                    throw;
                    //var ensureDbCreated = context.Database.EnsureCreated();
                    //if (!ensureDbCreated)
                        //throw new Exception("DB wasn't created. Ensure that your current user has access to your MSSQL server and that you do not use any password for this project.");
                }
                // Parse Account.json and seed initial data 
                // context.SaveChanges();
            }
        }
    }
}
