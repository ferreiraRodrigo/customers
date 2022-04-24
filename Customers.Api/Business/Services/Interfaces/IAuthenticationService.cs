using Customers.Business.Models;
using Customers.Presentation.Dtos.Authentication;
using System.Threading.Tasks;

namespace Customers.Business.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<OperationResult<AuthenticationTokenDTO>> Login(AuthenticationLoginDTO loginAuthenticationDTO);
    }
}
