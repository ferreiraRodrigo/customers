using Customers.Business.Models;
using Customers.Business.Repositories;
using Customers.Business.Services.Interfaces;
using Customers.Business.Services.OperationResults;
using Customers.Configurations;
using Customers.Presentation.Dtos.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerRepository _customerRepository;
        
        private SymmetricSecurityKey _symmetricSecurityKey;
        private SigningCredentials _signingCredentials;
        private string _issuer;
        private string _audience;
        private string _tokenType;
        private int _expirationTimeInSeconds;

        public AuthenticationService(ICustomerRepository customerRepository, 
            IOptions<AuthenticationTokenConfigurations> options)
        {
            _customerRepository = customerRepository;

            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
            _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
            _tokenType = options.Value.TokenType;
            _expirationTimeInSeconds = 1800;
        }

        public async Task<OperationResult<AuthenticationTokenDTO>> Login(AuthenticationLoginDTO loginAuthenticationDTO)
        {
            var customer = await _customerRepository.GetByEmailAndPassword(loginAuthenticationDTO.Email, loginAuthenticationDTO.Password);

            if (customer == null)
            {
                return new OperationResult<AuthenticationTokenDTO>(
                    null,
                    AuthenticationServiceOperationResults.AUTHENTICATION_EMAIL_OR_PASSWORD_INCORRECT,
                    "Incorrect email or password."
                );
            }

            return new OperationResult<AuthenticationTokenDTO>(GenerateToken(customer));
        }

        private AuthenticationTokenDTO GenerateToken(Customer customer)
        {
            var claims = new[]
            {
                new Claim(AuthenticationClaims.CustomerId, customer.Id.ToString()),
                new Claim(AuthenticationClaims.Name, customer.Name),
                new Claim(AuthenticationClaims.Email, customer.Email)
            };

            var tokenSettings = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddSeconds(_expirationTimeInSeconds),
                signingCredentials: _signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenSettings);

            return new AuthenticationTokenDTO
            {
                Token = token,
                TokenType = _tokenType,
                ExpiresIn = _expirationTimeInSeconds
            };
        }
    }
}
