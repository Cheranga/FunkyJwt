using System;
using System.IO;
using FunkyDI;
using FunkyDI.Configs;
using FunkyDI.Models;
using FunkyDI.QueryHandlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunkyDI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            

            var services = builder.Services;

            services.AddLogging();

            RegisterConfiguration(services);
            
            services.AddSingleton<SecurityHandler>();

            services.AddScoped<IQueryHandler<GetCustomerByIdQuery, Customer>, GetCustomerByIdHandler>();
        }

        private static void RegisterConfiguration(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
            var tokenSecurityKey = Environment.GetEnvironmentVariable("TokenSecurityKey");

            if (string.IsNullOrWhiteSpace(connectionString) || string.IsNullOrWhiteSpace(tokenSecurityKey))
            {
                throw new Exception("Invalid configuration");
            }

            
            services.AddSingleton(new DatabaseConfiguration{ConnectionString = connectionString});
            services.AddSingleton(new TokenConfiguration {SecurityKey = tokenSecurityKey});
        }
    }
}