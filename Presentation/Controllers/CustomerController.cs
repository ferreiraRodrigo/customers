using Customers.Business.Services.Interfaces;
using Customers.Business.Services.OperationResults;
using Customers.Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Customers.Presentation.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _customerService.GetCustomersAsync();

            return Ok(customers.Result);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomer(Guid customerId)
        {
            var customer = await _customerService.GetCustomerAsync(customerId);

            if (customer.Error == CustomerServiceOperationResults.CUSTOMER_NOT_FOUND)
            {
                return Problem(customer.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return Ok(customer.Result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDTO customerDTO)
        {
            var customer = await _customerService.CreateCustomerAsync(customerDTO);

            if (customer.Error == CustomerServiceOperationResults.CUSTOMER_EMAIL_ALREADY_EXISTS)
            {
                return Problem(customer.ErrorMessage, statusCode: (int)HttpStatusCode.Conflict);
            }

            return Created($"/customers/{customer.Result.Id}", customer.Result);
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDTO customerDTO, Guid customerId)
        {
            var customer = await _customerService.UpdateCustomerAsync(customerDTO, customerId);

            if (customer.Error == CustomerServiceOperationResults.CUSTOMER_NOT_FOUND)
            {
                return Problem(customer.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return Ok(customer.Result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var costumer = await _customerService.DeleteCustomerAsync(id);

            if (costumer.Error == CustomerServiceOperationResults.CUSTOMER_NOT_FOUND)
            {
                return Problem(costumer.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return NoContent();
        }

        [HttpGet("{customerId}/wishlist")]
        public async Task<IActionResult> GetCustomerWishList(Guid customerId)
        {
            var wishlist = await _customerService.GetCustomerWishListAsync(customerId);

            if (wishlist.Error == CustomerServiceOperationResults.CUSTOMER_NOT_FOUND)
            {
                return Problem(wishlist.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return Ok(wishlist.Result);
        }

        [HttpGet("{customerId}/wishlist/products/{productId}")]
        public async Task<IActionResult> GetCustomerWishListProduct(Guid customerId, Guid productId)
        {
            var product = await _customerService.GetProductFromCustomerWishListAsync(customerId, productId);

            if (product.Error == CustomerServiceOperationResults.CUSTOMER_NOT_FOUND || 
                product.Error == CustomerServiceOperationResults.PRODUCT_NOT_FOUND)
            {
                return Problem(product.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return Ok(product.Result);
        }

        [HttpPost("{customerId}/wishlist/products/{productId}")]
        public async Task<IActionResult> CreateProductOnCostumerWishList(Guid customerId, Guid productId)
        {
            var product = await _customerService.AddProductToCustomerWishListAsync(customerId, productId);

            if (product.Error == CustomerServiceOperationResults.CUSTOMER_NOT_FOUND ||
                product.Error == CustomerServiceOperationResults.PRODUCT_NOT_FOUND)
            {
                return Problem(product.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            if (product.Error == CustomerServiceOperationResults.PRODUCT_ALREADY_EXISTS)
            {
                return Problem(product.ErrorMessage, statusCode: (int)HttpStatusCode.Conflict);
            }

            return Created($"/customers/{customerId}/wishlist/products/{productId}", product.Result);
        }

        [HttpDelete("{customerId}/wishlist/products/{productId}")]
        public async Task<IActionResult> DeleteProductFromCostumerWishList(Guid customerId, Guid productId)
        {
            var product = await _customerService.DeleteProductFromCustomerWishListAsync(customerId, productId);

            if (product.Error == CustomerServiceOperationResults.CUSTOMER_NOT_FOUND ||
                product.Error == CustomerServiceOperationResults.PRODUCT_NOT_FOUND)
            {
                return Problem(product.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return NoContent();
        }
    }
}
