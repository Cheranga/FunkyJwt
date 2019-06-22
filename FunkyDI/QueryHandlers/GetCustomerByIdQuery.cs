namespace FunkyDI.QueryHandlers
{
    public class GetCustomerByIdQuery
    {
        public GetCustomerByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}