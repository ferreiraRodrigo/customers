using Customers.Business.Models;
using Customers.Business.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customers.Infra.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDataContext _dataContext;

        public CustomerRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Customer> GetAsync(Guid customerId)
        {
            return await _dataContext.Customers.FindAsync(customerId);
        }
        
        public async Task<Customer> CreateAsync(Customer customer)
        {
            customer.Email = customer.Email.ToLower();

            customer.SetCreatedAt();
            
            _dataContext.Customers.Add(customer);
            await _dataContext.SaveChangesAsync();

            return customer;
        }
        
        public async Task DeleteAsync(Guid customerId)
        {
            var customerToDelete = await _dataContext.Customers
                .Include(x => x.WishList)
                .ThenInclude(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == customerId);

            if (customerToDelete == null)
                throw new Exception("Customer not found");

            customerToDelete.SetDeletedAt();

            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            var customerToUpdate = await _dataContext.Customers.FindAsync(customer.Id);

            if (customerToUpdate == null)
                throw new Exception("Customer not found");

            customerToUpdate.SetUpdatedAt();

            _dataContext.Customers.Update(customer);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Customer> GetByEmail(string email)
        {
            email = email.ToLower();

            var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == email);

            return customer;
        }

        public async Task<Customer> GetByEmailAndPassword(string email, string password)
        {
            email = email.ToLower();

            var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == email && c.Password == password);

            return customer;
        }
    }
}
