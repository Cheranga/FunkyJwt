using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using FunkyDI.DTO;
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

        public LoginFunction(SecurityHandler securityHandler)
        {
            _securityHandler = securityHandler;
        }

        [FunctionName("login")]
        public async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "customers/login")]
            HttpRequest request, ILogger logger)
        {
            var loginDto = await request.FromBodyAsync<LoginDto>();
            if (loginDto == null)
            {
                return new BadRequestObjectResult("Invalid request data");
            }
            //
            // TODO: Validate username and password using the external provider and add some tokens if you require
            //
            var token = _securityHandler.GenerateToken(
                new Claim("deviceId", Guid.NewGuid().ToString()),
                new Claim("userId", Guid.NewGuid().ToString()));

            return new OkObjectResult(token);
        }
    }
}