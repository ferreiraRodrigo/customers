namespace Customers.Presentation.Dtos.Authentication
{
    public class AuthenticationTokenDTO
    {
        public string Token { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }
}
