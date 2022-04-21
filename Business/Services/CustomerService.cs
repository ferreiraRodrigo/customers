using Customers.Business.Models;
using Customers.Business.Repositories;
using Customers.Business.Services.OperationResults;
using Customers.Presentation.Dtos;
using System;
using System.Threading.Tasks;

namespace Customers.Business.Services.Interfaces
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IWishListRepository _wishlistRepository;

        public CustomerService(ICustomerRepository customerRepository, 
            IWishListRepository wishlistRepository)
        {
            _customerRepository = customerRepository;
            _wishlistRepository = wishlistRepository;
        }
        public async Task<OperationResult<Customer>> CreateCustomerAsync(CustomerCreationDTO customerDTO)
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
                Email = customerDTO.Email,
                Password = customerDTO.Password
            };

            var createdCustomer = await _customerRepository.CreateAsync(customer);
            
            var wishlist = new WishList
            {
                CustomerId = createdCustomer.Id
            };

            await _wishlistRepository.AddCustomerWishListAsync(wishlist);

            return new OperationResult<Customer>(createdCustomer);
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
        
        public async Task<OperationResult<Customer>> UpdateCustomerAsync(CustomerUpdateDTO customerDTO, Guid customerId)
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
    }
}
