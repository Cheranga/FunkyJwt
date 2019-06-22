using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FunkyDI
{
    public static class HttpExtensions
    {
        public static async Task<TResponse> FromBodyAsync<TResponse>(this HttpRequest request) where TResponse : class
        {
            if (request == null)
            {
                return null;
            }

            try
            {
                var content = await new StreamReader(request.Body).ReadToEndAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return null;
                }

                var response = JsonConvert.DeserializeObject<TResponse>(content);
                return response;
            }
            catch
            {
                return null;
            }
        }
    }
}