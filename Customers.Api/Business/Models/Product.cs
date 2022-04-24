using Customers.Business.Models.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Customers.Business.Models
{
    public class Product : EntityBase
    {
        [JsonPropertyName("id")]
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }
        [Required]
        [JsonIgnore]
        public Guid WishListId { get; set; }
    }
}
