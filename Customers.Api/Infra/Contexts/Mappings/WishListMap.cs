using Customers.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customers.Infra.Contexts.Mappings
{
    public class WishListMap : IEntityTypeConfiguration<WishList>
    {
        public void Configure(EntityTypeBuilder<WishList> builder)
        {
            builder.HasQueryFilter(c => !c.DeletedAt.HasValue);
        }
    }
}
