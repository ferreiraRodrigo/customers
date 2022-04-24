using Customers.Business.Services;
using Customers.Business.Services.Interfaces;
using Customers.Business.Services.Structs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Customers.Presentation.Controllers
{
    [ApiController]
    [Route("wishlist")]
    [Authorize]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;
        private readonly ILogger<WishListController> _logger;

        public WishListController(IWishListService wishListService,
            ILogger<WishListController> logger)
        {
            _wishListService = wishListService;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCustomerWishList()
        {
            if (!User.FindFirst(AuthenticationClaims.Scopes).Value.Contains(AuthenticationScopes.ReadWishList))
            {
                return Forbid();
            }

            var customerId = Guid.Parse(User.FindFirst(AuthenticationClaims.CustomerId).Value);
            var wishlist = await _wishListService.GetCustomerWishListAsync(customerId);

            if (wishlist.Error == WishListServiceOperationResults.CUSTOMER_NOT_FOUND)
            {
                _logger.LogInformation($"Customer with {customerId} id not found. Message: {wishlist.ErrorMessage}");
                return Problem(wishlist.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return Ok(wishlist.Result);
        }

        [HttpGet("products/{productId}")]
        public async Task<IActionResult> GetCustomerWishListProduct(Guid productId)
        {
            if (!User.FindFirst(AuthenticationClaims.Scopes).Value.Contains(AuthenticationScopes.ReadWishList))
            {
                return Forbid();
            }

            var customerId = Guid.Parse(User.FindFirst(AuthenticationClaims.CustomerId).Value);
            var product = await _wishListService.GetProductFromCustomerWishListAsync(customerId, productId);

            if (product.Error == WishListServiceOperationResults.CUSTOMER_NOT_FOUND)
            {
                _logger.LogInformation($"Customer with {customerId} id not found. Message: {product.ErrorMessage}");
                return Problem(product.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            if (product.Error == WishListServiceOperationResults.PRODUCT_NOT_FOUND)
            {
                _logger.LogInformation($"Product with {productId} id not found. Message: {product.ErrorMessage}");
                return Problem(product.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return Ok(product.Result);
        }

        [HttpPost("products/{productId}")]
        public async Task<IActionResult> CreateProductOnCostumerWishList(Guid productId)
        {
            if (!User.FindFirst(AuthenticationClaims.Scopes).Value.Contains(AuthenticationScopes.WriteWishList))
            {
                return Forbid();
            }

            var customerId = Guid.Parse(User.FindFirst(AuthenticationClaims.CustomerId).Value);
            var product = await _wishListService.AddProductToCustomerWishListAsync(customerId, productId);

            if (product.Error == WishListServiceOperationResults.CUSTOMER_NOT_FOUND)
            {
                _logger.LogInformation($"Customer with {customerId} id not found. Message: {product.ErrorMessage}");
                return Problem(product.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            if (product.Error == WishListServiceOperationResults.PRODUCT_NOT_FOUND)
            {
                _logger.LogInformation($"Product with {productId} id not found. Message: {product.ErrorMessage}");
                return Problem(product.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            if (product.Error == WishListServiceOperationResults.PRODUCT_ALREADY_EXISTS)
            {
                _logger.LogInformation($"Product with {productId} id already exists. Message: {product.ErrorMessage}");
                return Problem(product.ErrorMessage, statusCode: (int)HttpStatusCode.Conflict);
            }

            return Created($"/customers/{customerId}/wishlist/products/{productId}", product.Result);
        }

        [HttpDelete("products/{productId}")]
        public async Task<IActionResult> DeleteProductFromCostumerWishList(Guid productId)
        {
            if (!User.FindFirst(AuthenticationClaims.Scopes).Value.Contains(AuthenticationScopes.WriteWishList))
            {
                return Forbid();
            }

            var customerId = Guid.Parse(User.FindFirst(AuthenticationClaims.CustomerId).Value);            
            var product = await _wishListService.DeleteProductFromCustomerWishListAsync(customerId, productId);

            if (product.Error == WishListServiceOperationResults.CUSTOMER_NOT_FOUND)
            {
                _logger.LogInformation($"Customer with {customerId} id not found. Message: {product.ErrorMessage}");
                return Problem(product.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            if (product.Error == WishListServiceOperationResults.PRODUCT_NOT_FOUND)
            {
                _logger.LogInformation($"Product with {productId} id not found. Message: {product.ErrorMessage}");
                return Problem(product.ErrorMessage, statusCode: (int)HttpStatusCode.NotFound);
            }

            return NoContent();
        }
    }
}
