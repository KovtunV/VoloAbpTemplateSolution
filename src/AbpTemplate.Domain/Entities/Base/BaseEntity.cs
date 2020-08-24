using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace AbpTemplate.Domain.Entities.Base
{
    public abstract class BaseEntity : Entity, IHasCreationTime, IHasModificationTime
    {
        // I must have "Id" setter opened because of the fastest BulkInsert from Npgsql.Bulk
        public int Id { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        protected BaseEntity()
        {
            
        }

        protected BaseEntity(int id)
        {
            Id = id;
        }

        public override object[] GetKeys()
        {
            return new object[] { Id };
        }
    }
}
