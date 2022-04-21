using Customers.Business.Services.Interfaces;
using Customers.Business.Services.OperationResults;
using Customers.Presentation.Dtos.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Customers.Presentation.Controllers
{
    [ApiController]
    [Route("authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticationLoginDTO loginAuthenticationDTO)
        {
            var loginResult = await _authenticationService.Login(loginAuthenticationDTO);
            
            if (loginResult.Error == AuthenticationServiceOperationResults.AUTHENTICATION_EMAIL_OR_PASSWORD_INCORRECT)
            {
                return Problem(loginResult.ErrorMessage, statusCode: (int)HttpStatusCode.Unauthorized);
            }

            return Ok(loginResult.Result);
        }
    }
}
