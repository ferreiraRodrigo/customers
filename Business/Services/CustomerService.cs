using Customers.Business.Models;
using Customers.Business.Repositories;
using Customers.Business.Services.OperationResults;
using Customers.Infra.Adapters;
using Customers.Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customers.Business.Services.Interfaces
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IWishListRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductAdapter _productAdapter;

        public CustomerService(ICustomerRepository customerRepository, 
            IWishListRepository wishlistRepository, 
            IProductRepository productRepository,
            IProductAdapter productAdapter)
        {
            _customerRepository = customerRepository;
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
            _productAdapter = productAdapter;
        }

        public async Task<OperationResult<IEnumerable<Customer>>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            return new OperationResult<IEnumerable<Customer>>(customers);
        }
        
        public async Task<OperationResult<Customer>> GetCustomerAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetAsync(customerId);

            if (customer == null)
            {
                return new OperationResult<Customer>(
                    customer,
                    CustomerServiceOperationResults.CUSTOMER_NOT_FOUND,
                    "Customer not found."
                );
            }

            return new OperationResult<Customer>(customer);
        }
        
        public async Task<OperationResult<Customer>> CreateCustomerAsync(CreateCustomerDTO customerDTO)
        {
            var existingCustomer = await _customerRepository.GetByEmail(customerDTO.Email);

            if (existingCustomer != null)
            {
                return new OperationResult<Customer>(
                    existingCustomer,
                    CustomerServiceOperationResults.CUSTOMER_EMAIL_ALREADY_EXISTS,
                    "Customer already exists."
                );
            }

            var customer = new Customer
            {
                Name = customerDTO.Name,
                Email = customerDTO.Email
            };

            var createdCustomer = await _customerRepository.CreateAsync(customer);
            
            var wishlist = new WishList
            {
                CustomerId = createdCustomer.Id
            };

            await _wishlistRepository.AddCustomerWishListAsync(wishlist);

            return new OperationResult<Customer>(createdCustomer);
        }

        public async Task<OperationResult<Customer>> UpdateCustomerAsync(UpdateCustomerDTO customerDTO, Guid customerId)
        {
            var customer = await _customerRepository.GetAsync(customerId);

            if (customer == null)
            {
                return new OperationResult<Customer>(
                    customer,
                    CustomerServiceOperationResults.CUSTOMER_NOT_FOUND,
                    "Customer not found."
                );
            }

            customer.Name = customerDTO.Name;
            
            await _customerRepository.UpdateAsync(customer);

            return new OperationResult<Customer>(customer);
        }

        public async Task<OperationResult<Customer>> DeleteCustomerAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetAsync(customerId);

            if (customer == null)
            {
                return new OperationResult<Customer>(
                    customer,
                    CustomerServiceOperationResults.CUSTOMER_NOT_FOUND,
                    "Customer not found."
                );
            }

            await _customerRepository.DeleteAsync(customerId);

            return new OperationResult<Customer>(customer);
        }

        public async Task<OperationResult<WishList>> GetCustomerWishListAsync(Guid customerId)
        {
            var wishlist = await GetCustomerWishList(customerId);

            if (wishlist == null)
            {
                return new OperationResult<WishList>(
                    wishlist,
                    CustomerServiceOperationResults.CUSTOMER_NOT_FOUND,
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
                    CustomerServiceOperationResults.CUSTOMER_NOT_FOUND,
                    "Customer not found."
                );
            }

            var product = await _productRepository.GetAsync(productId, wishlist.Id);

            if (product != null)
            {
                return new OperationResult<Product>(
                    product,
                    CustomerServiceOperationResults.PRODUCT_ALREADY_EXISTS,
                    "Product already exists in this customer wishlist."
                );
            }

            var productToCreate = _productAdapter.GetProduct(productId);

            if (productToCreate.Result == null)
            {
                return new OperationResult<Product>(
                    null,
                    CustomerServiceOperationResults.PRODUCT_NOT_FOUND,
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
                    CustomerServiceOperationResults.CUSTOMER_NOT_FOUND,
                    "Customer not found."
                );
            }
            var product = await _productRepository.GetAsync(productId, wishlist.Id);

            if (product == null)
            {
                return new OperationResult<Product>(
                    product,
                    CustomerServiceOperationResults.PRODUCT_NOT_FOUND,
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
                    CustomerServiceOperationResults.CUSTOMER_NOT_FOUND,
                    "Customer not found."
                );
            }

            var product = await _productRepository.GetAsync(productId, wishlist.Id);

            if (product == null)
            {
                return new OperationResult<Product>(
                    product,
                    CustomerServiceOperationResults.PRODUCT_NOT_FOUND,
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
