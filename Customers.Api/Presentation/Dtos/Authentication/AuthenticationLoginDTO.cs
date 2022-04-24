namespace Customers.Presentation.Dtos.Authentication
{
    public class AuthenticationLoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Scopes { get; set; }
    }
}
