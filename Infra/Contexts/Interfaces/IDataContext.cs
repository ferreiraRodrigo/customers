using Customers.Business.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Customers.Infra
{
    public interface IDataContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<WishList> WishLists { get; set; }
        DbSet<Product> Products { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
