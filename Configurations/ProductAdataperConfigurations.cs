using System.ComponentModel.DataAnnotations;

namespace Customers.Infra.Adapters.Product
{
    public record ProductAdataperConfigurations
    {
        public const string CONFIG_NAME = "ProductAdapter";
        [Required]
        public string BaseAddress { get; set; } = null;
        [Required]
        public string Resource { get; set; } = null;
    }
}
