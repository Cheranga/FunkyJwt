namespace FunkyDI.QueryHandlers
{
    public class GetAuthorizationsForUserByIdQuery
    {
        public int Id { get; }

        public GetAuthorizationsForUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}