using Customers.Business.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Customers.Business.Models
{
    public class WishList : EntityBase
    {
        public ICollection<Product> Products { get; set; } = new List<Product>();
        [Required]
        public Guid CustomerId { get; set; }

        public override void SetDeletedAt()
        {
            UpdatedAt = DateTimeOffset.Now;
            DeletedAt = DateTimeOffset.Now;

            foreach (var product in Products)
            {
                product.SetDeletedAt();
            }
        }        
    }
}
