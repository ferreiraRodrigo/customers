using System.ComponentModel.DataAnnotations;

namespace Customers.Configurations
{
    public record AuthenticationTokenConfigurations
    {
        public const string CONFIG_NAME = "Jwt";
        [Required]
        public string Key { get; set; } = null;
        [Required]
        public string Issuer { get; set; } = null;
        [Required]
        public string TokenType { get; set; } = null;
        [Required]
        public string Audience { get; set; } = null;
    }
}
