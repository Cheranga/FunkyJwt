using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FunkyDI.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FunkyDI
{
    public class SecurityHandler
    {
        private const string Authorization = nameof(Authorization);

        private readonly byte[] _key;
        private readonly ILogger _logger;

        public SecurityHandler(TokenConfiguration config, ILogger logger)
        {
            if (config == null || string.IsNullOrEmpty(config.SecurityKey))
            {
                throw new ArgumentNullException("config");
            }

            _logger = logger;

            _key = Convert.FromBase64String(config.SecurityKey);
        }

        public string GenerateToken(params Claim[] claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddYears(1),
                Subject = new ClaimsIdentity(new List<Claim>(claims)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            var token = jwtTokenHandler.WriteToken(jwtToken);

            return token;
        }

        public ClaimsPrincipal GetPrincipal(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }

            request.Headers.TryGetValue(Authorization, out var someOtherValue);


            return GetPrincipal(someOtherValue.ToString());
        }

        public bool IsAuthenticated(string bearerToken)
        {
            return GetPrincipal(bearerToken) != null;
        }

        public TResponse To<TResponse>(HttpRequest request) where TResponse : class
        {
            var principal = GetPrincipal(request);
            if (principal == null)
            {
                return null;
            }

            var obj = new ExpandoObject();
            var dictionary = (IDictionary<string, object>) obj;

            foreach (var claim in principal.Claims)
            {
                dictionary.Add(claim.Type, claim.Value);
            }

            var serializedContent = JsonConvert.SerializeObject(obj);

            var deserialized = JsonConvert.DeserializeObject<TResponse>(serializedContent);

            return deserialized;
        }

        private ClaimsPrincipal GetPrincipal(string bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return null;
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(_key)
            };

            bearerToken = bearerToken.Replace(JwtBearerDefaults.AuthenticationScheme, string.Empty, StringComparison.OrdinalIgnoreCase).Trim();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(bearerToken, tokenValidationParameters, out var validatedToken);

                if (claimsPrincipal == null || validatedToken == null)
                {
                    return null;
                }

                return claimsPrincipal;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Cannot verify the token: {exception}");
            }

            return null;
        }
    }
}