using System;

namespace Customers.Infra.Adapters.Product
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
        public string Brand { get; set; }
    }
}
