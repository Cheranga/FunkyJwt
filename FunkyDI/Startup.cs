using FunkyDI;
using FunkyDI.Models;
using FunkyDI.QueryHandlers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace FunkyDI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IQueryHandler<GetCustomerByIdQuery, Customer>, GetCustomerByIdHandler>();
        }
    }
}