using Customers.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customers.Business.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetAsync(Guid customerId);
        Task<Customer> CreateAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Guid customerId);
        Task<Customer> GetByEmail(string email);
    }
}
