using Customers.Business.Models;
using Customers.Presentation.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customers.Business.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<OperationResult<Customer>> GetCustomerAsync(Guid id);
        Task<OperationResult<IEnumerable<Customer>>> GetCustomersAsync();
        Task<OperationResult<Customer>> CreateCustomerAsync(CreateCustomerDTO customerDTO);
        Task<OperationResult<Customer>> UpdateCustomerAsync(UpdateCustomerDTO customerDTO, Guid id);
        Task<OperationResult<Customer>> DeleteCustomerAsync(Guid id);
        Task<OperationResult<WishList>> GetCustomerWishListAsync(Guid customerId);
        Task<OperationResult<Product>> AddProductToCustomerWishListAsync(Guid customerId, Guid productId);
        Task<OperationResult<Product>> GetProductFromCustomerWishListAsync(Guid customerId, Guid productId);
        Task<OperationResult<Product>> DeleteProductFromCustomerWishListAsync(Guid customerId, Guid productId);
    }
}
