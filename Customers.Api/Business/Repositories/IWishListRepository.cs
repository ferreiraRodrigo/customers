using Customers.Business.Models;
using System;
using System.Threading.Tasks;

namespace Customers.Business.Repositories
{
    public interface IWishListRepository
    {
        Task AddCustomerWishListAsync(WishList wishList);
        Task<WishList> GetCustomerWishListAsync(Guid customerId);
    }
}
