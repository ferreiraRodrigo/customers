using Customers.Business.Models;
using Customers.Business.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.Infra.Repositories
{
    public class WishListRepository : IWishListRepository
    {
        private readonly IDataContext _dataContext;
        public WishListRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddCustomerWishListAsync(WishList wishList)
        {
            wishList.SetCreatedAt();
            
            _dataContext.WishLists.Add(wishList);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<WishList> GetCustomerWishListAsync(Guid customerId)
        {
            return await _dataContext.WishLists.Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.CustomerId == customerId);
        }
    }
}
