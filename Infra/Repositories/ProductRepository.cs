using Customers.Business.Models;
using Customers.Business.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Customers.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDataContext _dataContext;

        public ProductRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task<Product> AddAsync(Product product)
        {
            product.SetCreatedAt();

            _dataContext.Products.Add(product);
            await _dataContext.SaveChangesAsync();

            return product;
        }

        public async Task DeleteAsync(Guid productId, Guid wishlistId)
        {
            var product = await _dataContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId && x.WishListId == wishlistId);

            if (product == null)
                throw new Exception("Product not found");

            product.SetDeletedAt();

            await _dataContext.SaveChangesAsync();
        }
        
        public async Task<Product> GetAsync(Guid productId, Guid wishlistId)
        {
            return await _dataContext.Products.FirstOrDefaultAsync(x => x.ProductId == productId && x.WishListId == wishlistId);
        }
    }
}
