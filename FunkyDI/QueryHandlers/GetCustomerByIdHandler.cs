using System.Threading.Tasks;
using FunkyDI.Models;

namespace FunkyDI.QueryHandlers
{
    public class GetCustomerByIdHandler : IQueryHandler<GetCustomerByIdQuery, Customer>
    {
        public Task<Customer> HandleAsync(GetCustomerByIdQuery query)
        {
            if (query == null || query.Id <= 0)
            {
                return Task.FromResult<Customer>(null);
            }

            //
            // Let's simulate that if the requested id is even, we return an actual customer, otherwise we return as null
            //
            if (query.Id % 2 == 0)
            {
                return Task.FromResult(new Customer {Id = query.Id, Name = $"Customer {query.Id}", Address = $"Address {query.Id}"});
            }

            return Task.FromResult<Customer>(null);
        }
    }
}