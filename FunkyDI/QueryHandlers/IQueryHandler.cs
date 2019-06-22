using System.Threading.Tasks;

namespace FunkyDI.QueryHandlers
{
    public interface IQueryHandler<in TQuery, TResponse> where TQuery : class where TResponse : class
    {
        Task<TResponse> HandleAsync(TQuery query);
    }
}