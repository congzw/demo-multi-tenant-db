using System;
using System.Linq;
using DemoApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Config.CreateServiceProvider();

            //default tenant
            using (var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var seedService = provider.GetService<SeedService>();

                seedService.Seed();

                var dbContext = scope.ServiceProvider.GetRequiredService<DemoDbContext>();
                var users = dbContext.Users.ToList();

                foreach (var user in users)
                {
                    Console.WriteLine("{0},{1},{2}", user.Id, user.Username, user.FromSource);
                }
            }

            //switch tenant
            using (var scope = serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var tenantContext = provider.GetService<TenantContext>();
                tenantContext.Tenant = "A";

                var seedService = provider.GetService<SeedService>();
                seedService.Seed();

                var dbContext = scope.ServiceProvider.GetRequiredService<DemoDbContext>();
                var users = dbContext.Users.ToList();

                foreach (var user in users)
                {
                    Console.WriteLine("{0},{1},{2}", user.Id, user.Username, user.FromSource);
                }
            }

            Console.WriteLine("COMPLETE!");
            Console.Read();
        }
    }
}
