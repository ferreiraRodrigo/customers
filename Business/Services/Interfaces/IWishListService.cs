using Customers.Business.Models;
using System;
using System.Threading.Tasks;

namespace Customers.Business.Services.Interfaces
{
    public interface IWishListService
    {
        Task<OperationResult<WishList>> GetCustomerWishListAsync(Guid customerId);
        Task<OperationResult<Product>> AddProductToCustomerWishListAsync(Guid customerId, Guid productId);
        Task<OperationResult<Product>> GetProductFromCustomerWishListAsync(Guid customerId, Guid productId);
        Task<OperationResult<Product>> DeleteProductFromCustomerWishListAsync(Guid customerId, Guid productId);
    }
}
