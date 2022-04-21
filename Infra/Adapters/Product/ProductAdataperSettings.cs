using System.ComponentModel.DataAnnotations;

namespace Customers.Infra.Adapters.Product
{
    public record ProductAdataperSettings
    {
        public const string CONFIG_NAME = "ProductAdapter";
        [Required]
        public string BaseAddress { get; set; } = null;
        public string Resource { get; set; } = null;
    }
}
