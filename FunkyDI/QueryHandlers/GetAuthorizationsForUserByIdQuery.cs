namespace FunkyDI.QueryHandlers
{
    public class GetAuthorizationsForUserByIdQuery
    {
        public GetAuthorizationsForUserByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}