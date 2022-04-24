using Customers.Infra.Adapters.Product;
using System;
using System.Threading.Tasks;

namespace Customers.Infra.Adapters
{
    public interface IProductAdapter
    {
        Task<ProductDTO> GetProduct(Guid id);
    }
}
