using Customers.Business.Services;
using Customers.Business.Services.Interfaces;
using Customers.Business.Services.OperationResults;
using Customers.Business.Services.Structs;
using Customers.Presentation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Customers.Presentation.Controllers
{
    [ApiController]
    [Route("customers")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreationDTO customerDTO)
        {
            var customer = await _customerService.CreateCustomerAsync(customerDTO);

            if (customer.Error == CustomerServiceOperationResults.CUSTOMER_EMAIL_ALREADY_EXISTS)
            {
                return Problem(customer.ErrorMessage, statusCode: (int)HttpStatusCode.Conflict);
            }

            return Created($"/customers/{customer.Result.Id}", customer.Result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            if (!User.FindFirst(AuthenticationClaims.Scopes).Value.Contains(AuthenticationScopes.ReadCustomer))
            {
                return Forbid();
            }

            var customerId = Guid.Parse(User.FindFirst(AuthenticationClaims.CustomerId).Value);
            var customer = await _customerService.GetCustomerAsync(customerId);

            if (customer.Error == CustomerServiceOperationResults.CUSTOMER_NOT_FOUND)
            {
                return Problem(customer.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return Ok(customer.Result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(CustomerUpdateDTO customerDTO)
        {
            if (!User.FindFirst(AuthenticationClaims.Scopes).Value.Contains(AuthenticationScopes.WriteCustomer))
            {
                return Forbid();
            }
            
            var customerId = Guid.Parse(User.FindFirst(AuthenticationClaims.CustomerId).Value);
            var customer = await _customerService.UpdateCustomerAsync(customerDTO, customerId);

            if (customer.Error == CustomerServiceOperationResults.CUSTOMER_NOT_FOUND)
            {
                return Problem(customer.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return Ok(customer.Result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer()
        {
            if (!User.FindFirst(AuthenticationClaims.Scopes).Value.Contains(AuthenticationScopes.WriteCustomer))
            {
                return Forbid();
            }
            
            var customerId = Guid.Parse(User.FindFirst(AuthenticationClaims.CustomerId).Value);
            var costumer = await _customerService.DeleteCustomerAsync(customerId);

            if (costumer.Error == CustomerServiceOperationResults.CUSTOMER_NOT_FOUND)
            {
                return Problem(costumer.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return NoContent();
        }
    }
}
