using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FunkyDI.Models;
using FunkyDI.QueryHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace FunkyDI.Functions
{
    public class GetCustomerFunction
    {
        private readonly IQueryHandler<GetCustomerByIdQuery, Customer> _queryHandler;
        private readonly SecurityHandler _securityHandler;
        private readonly ILogger _logger;

        public GetCustomerFunction(IQueryHandler<GetCustomerByIdQuery, Customer> queryHandler, 
            SecurityHandler securityHandler)
        {
            _queryHandler = queryHandler;
            _securityHandler = securityHandler;
        }

        [FunctionName("GetCustomer")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customers/find/{id}")]
            HttpRequest request, int id,
            ILogger logger)
        {
            logger.LogInformation($"Invoked {nameof(GetCustomerFunction)}");

            var userInformation = _securityHandler.To<AllowedFeatureCollection>(request, FeatureConstants.AllowedFeaturesClaim);

            if (userInformation?.Features == null || !userInformation.Features.Any())
            {
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }

            var hasAccess = userInformation.Features.FirstOrDefault(x => x.FeatureId == FeatureConstants.Customers) != null;

            if (!hasAccess)
            {
                return new StatusCodeResult((int) HttpStatusCode.Unauthorized);
            }

            var customer = await _queryHandler.HandleAsync(new GetCustomerByIdQuery(id));
            if (customer == null)
            {
                logger.LogWarning($"Customer not found: {id}");
                return new NotFoundResult();
            }

            var data = new
            {
                customer.Id,
                customer.Name,
                customer.Address
            };

            return new OkObjectResult(data);
        }
    }
}