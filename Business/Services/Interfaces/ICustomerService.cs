using Customers.Business.Models;
using Customers.Presentation.Dtos;
using System;
using System.Threading.Tasks;

namespace Customers.Business.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<OperationResult<Customer>> GetCustomerAsync(Guid id);
        Task<OperationResult<Customer>> CreateCustomerAsync(CustomerCreationDTO customerDTO);
        Task<OperationResult<Customer>> UpdateCustomerAsync(CustomerUpdateDTO customerDTO, Guid id);
        Task<OperationResult<Customer>> DeleteCustomerAsync(Guid id);
    }
}
