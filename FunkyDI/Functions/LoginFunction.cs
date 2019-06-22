using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using FunkyDI.DTO;
using FunkyDI.QueryHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FunkyDI.Functions
{
    public class LoginFunction
    {
        private readonly SecurityHandler _securityHandler;
        private readonly IQueryHandler<GetAuthorizationsForUserByIdQuery, AllowedFeatureCollection> _getAuthorizationsHandler;

        public LoginFunction(SecurityHandler securityHandler, IQueryHandler<GetAuthorizationsForUserByIdQuery, AllowedFeatureCollection> getAuthorizationsHandler)
        {
            _securityHandler = securityHandler;
            _getAuthorizationsHandler = getAuthorizationsHandler;
        }

        [FunctionName("login")]
        public async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "customers/login")]
            HttpRequest request, ILogger logger)
        {
            var loginDto = await request.FromBodyAsync<LoginDtoRequest>();
            if (loginDto == null)
            {
                return new BadRequestObjectResult("Invalid request data");
            }

            var id = loginDto.UserName.Equals("cheranga", StringComparison.OrdinalIgnoreCase) ? 1 : 2;

            var allowedFeatures = await _getAuthorizationsHandler.HandleAsync(new GetAuthorizationsForUserByIdQuery(id));
            
            var token = _securityHandler.GenerateToken(
                new Claim(FeatureConstants.AllowedFeaturesClaim, JsonConvert.SerializeObject(allowedFeatures)));

            return new OkObjectResult(token);
        }
    }
}