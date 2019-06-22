namespace FunkyDI.DTO
{
    public class LoginDtoResponse
    {
        public string Token { get; }

        public LoginDtoResponse(string token)
        {
            Token = token;
        }
    }
}