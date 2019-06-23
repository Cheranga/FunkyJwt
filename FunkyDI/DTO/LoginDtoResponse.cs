namespace FunkyDI.DTO
{
    public class LoginDtoResponse
    {
        public LoginDtoResponse(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}