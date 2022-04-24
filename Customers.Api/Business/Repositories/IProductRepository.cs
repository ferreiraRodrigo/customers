using Customers.Business.Models;
using System;
using System.Threading.Tasks;

namespace Customers.Business.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetAsync(Guid productId, Guid wishlistId);
        Task<Product> AddAsync(Product product);
        Task DeleteAsync(Guid productId, Guid wishlistId);
    }
}
