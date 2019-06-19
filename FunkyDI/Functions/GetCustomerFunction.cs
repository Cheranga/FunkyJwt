using System.Threading.Tasks;
using FunkyDI.Models;
using FunkyDI.QueryHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunkyDI.Functions
{
    public class GetCustomerFunction
    {
        private readonly IQueryHandler<GetCustomerByIdQuery, Customer> _queryHandler;

        public GetCustomerFunction(IQueryHandler<GetCustomerByIdQuery, Customer> queryHandler)
        {
            _queryHandler = queryHandler;
        }

        [FunctionName("GetCustomer")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customers/{id}")]
            HttpRequest request, int id,
            ILogger logger)
        {
            logger.LogInformation($"Invoked {nameof(GetCustomerFunction)}");

            var customer = await _queryHandler.HandleAsync(new GetCustomerByIdQuery(id));
            if (customer == null)
            {
                logger.LogWarning($"Customer not found: {id}");
                return new NotFoundResult();
            }

            return new OkObjectResult(customer);
        }
    }
}