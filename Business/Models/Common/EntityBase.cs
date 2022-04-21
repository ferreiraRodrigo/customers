using System;
using System.Text.Json.Serialization;

namespace Customers.Business.Models.Common
{
    public abstract class EntityBase
    {
        [JsonIgnore]
        public virtual Guid Id { get; set; }
        [JsonIgnore]
        public virtual DateTimeOffset CreatedAt { get; set; }
        [JsonIgnore]
        public virtual DateTimeOffset UpdatedAt { get; set; }
        [JsonIgnore]
        public virtual DateTimeOffset? DeletedAt { get; set; }

        public virtual void SetCreatedAt()
        {
            CreatedAt = DateTimeOffset.Now;
            UpdatedAt = DateTimeOffset.Now;
        }

        public virtual void SetUpdatedAt()
        {
            UpdatedAt = DateTimeOffset.Now;
        }

        public virtual void SetDeletedAt()
        {
            UpdatedAt = DateTimeOffset.Now;
            DeletedAt = DateTimeOffset.Now;
        }
    }
}
