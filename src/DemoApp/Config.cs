using System;
using System.IO;
using DemoApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApp
{
    public static class Config
    {
        public static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddMyServices();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        public static IServiceCollection AddMyServices(this IServiceCollection services)
        {
            services.AddDbContext<DemoDbContext>((provider, builder) =>
            {
                var tenantContext = provider.GetService<TenantContext>();
                if (string.IsNullOrWhiteSpace(tenantContext.Tenant))
                {
                    builder.UseSqlite(string.Format("Data Source={0}", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoDb.db"))); 
                    return;
                }
                var connString = string.Format("Data Source={0}", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DemoDb_" + tenantContext.Tenant + ".db"));
                builder.UseSqlite(connString);
            });
            
            services.AddScoped<SeedService>();
            services.AddSingleton<TenantContext>();

            return services;
        }
    }
}
