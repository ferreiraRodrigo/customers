using Customers.Business.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Customers.Business.Models
{
    public class Customer : EntityBase
    {
        public override Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string Scopes { get; set; }
        [JsonIgnore]
        public WishList WishList { get; set; }

        public override void SetDeletedAt()
        {
            UpdatedAt = DateTimeOffset.Now;
            DeletedAt = DateTimeOffset.Now;

            WishList.SetDeletedAt();
        }

        public string[] GetScopes()
        {
            return Scopes.Split(' ');
        }
    }
}
