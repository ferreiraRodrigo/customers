using Customers.Business.Models;
using Customers.Business.Repositories;
using Customers.Business.Services.Interfaces;
using Customers.Business.Services.Structs;
using Customers.Infra.Adapters;
using System;
using System.Threading.Tasks;

namespace Customers.Business.Services
{
    public class WishListService : IWishListService
    {
        private readonly ICustomerRepository _customerRepository;        
        private readonly IWishListRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductAdapter _productAdapter;

        public WishListService(IWishListRepository wishlistRepository, 
            IProductRepository productRepository, 
            IProductAdapter productAdapter,
            ICustomerRepository customerRepository)
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
            _productAdapter = productAdapter;
            _customerRepository = customerRepository;
        }
        public async Task<OperationResult<WishList>> GetCustomerWishListAsync(Guid customerId)
        {
            var wishlist = await GetCustomerWishList(customerId);

            if (wishlist == null)
            {
                return new OperationResult<WishList>(
                    wishlist,
                    WishListServiceOperationResults.CUSTOMER_NOT_FOUND,
                    "Customer not found."
                );
            }

            return new OperationResult<WishList>(wishlist);
        }

        public async Task<OperationResult<Product>> AddProductToCustomerWishListAsync(Guid customerId, Guid productId)
        {
            var wishlist = await GetCustomerWishList(customerId);

            if (wishlist == null)
            {
                return new OperationResult<Product>(
                    null,
                    WishListServiceOperationResults.CUSTOMER_NOT_FOUND,
                    "Customer not found."
                );
            }

            var product = await _productRepository.GetAsync(productId, wishlist.Id);

            if (product != null)
            {
                return new OperationResult<Product>(
                    product,
                    WishListServiceOperationResults.PRODUCT_ALREADY_EXISTS,
                    "Product already exists in this customer wishlist."
                );
            }

            var productToCreate = _productAdapter.GetProduct(productId);

            if (productToCreate.Result == null)
            {
                return new OperationResult<Product>(
                    null,
                    WishListServiceOperationResults.PRODUCT_NOT_FOUND,
                    "Product not found."
                );
            }

            product = new Product
            {
                ProductId = productToCreate.Result.Id,
                Title = productToCreate.Result.Title,
                Price = productToCreate.Result.Price,
                Image = productToCreate.Result.Image,
                WishListId = wishlist.Id
            };

            await _productRepository.AddAsync(product);

            return new OperationResult<Product>(product);
        }

        public async Task<OperationResult<Product>> GetProductFromCustomerWishListAsync(Guid customerId, Guid productId)
        {
            var wishlist = await GetCustomerWishList(customerId);

            if (wishlist == null)
            {
                return new OperationResult<Product>(
                    null,
                    WishListServiceOperationResults.CUSTOMER_NOT_FOUND,
                    "Customer not found."
                );
            }
            var product = await _productRepository.GetAsync(productId, wishlist.Id);

            if (product == null)
            {
                return new OperationResult<Product>(
                    product,
                    WishListServiceOperationResults.PRODUCT_NOT_FOUND,
                    "Product not found."
                );
            }

            return new OperationResult<Product>(product);
        }

        public async Task<OperationResult<Product>> DeleteProductFromCustomerWishListAsync(Guid customerId, Guid productId)
        {
            var wishlist = await GetCustomerWishList(customerId);

            if (wishlist == null)
            {
                return new OperationResult<Product>(
                    null,
                    WishListServiceOperationResults.CUSTOMER_NOT_FOUND,
                    "Customer not found."
                );
            }

            var product = await _productRepository.GetAsync(productId, wishlist.Id);

            if (product == null)
            {
                return new OperationResult<Product>(
                    product,
                    WishListServiceOperationResults.PRODUCT_NOT_FOUND,
                    "Product not found."
                );
            }

            await _productRepository.DeleteAsync(productId, wishlist.Id);

            return new OperationResult<Product>(product);
        }
        
        private async Task<WishList> GetCustomerWishList(Guid customerId)
        {
            var customer = await _customerRepository.GetAsync(customerId);

            if (customer == null)
            {
                return null;
            }

            var wishlist = _wishlistRepository.GetCustomerWishListAsync(customerId);

            return wishlist.Result;
        }
    }
}
