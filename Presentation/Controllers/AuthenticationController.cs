using Customers.Business.Services.Interfaces;
using Customers.Business.Services.OperationResults;
using Customers.Presentation.Dtos.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Customers.Presentation.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService,
            ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationLoginDTO loginAuthenticationDTO)
        {
            var loginResult = await _authenticationService.Login(loginAuthenticationDTO);
            
            if (loginResult.Error == AuthenticationServiceOperationResults.AUTHENTICATION_EMAIL_OR_PASSWORD_INCORRECT)
            {
                _logger.LogInformation($"Authentication failed for the customer: {loginAuthenticationDTO.Email}. Message: {loginResult.ErrorMessage}");
                return Problem(loginResult.ErrorMessage, statusCode: (int)HttpStatusCode.Unauthorized);
            }

            if (loginResult.Error == AuthenticationServiceOperationResults.AUTHENTICATION_INVALID_SCOPE)
            {
                _logger.LogInformation($"Authorization failed for the customer: {loginAuthenticationDTO.Email}. Message: {loginResult.ErrorMessage}");
                return Problem(loginResult.ErrorMessage, statusCode: (int)HttpStatusCode.Forbidden);
            }

            return Ok(loginResult.Result);
        }
    }
}
