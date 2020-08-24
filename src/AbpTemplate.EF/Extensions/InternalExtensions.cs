using AbpTemplate.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace AbpTemplate.EF.Extensions
{
    internal static class InternalExtensions
    {
        /// <summary>
        /// Adds sequence and sets default value
        /// </summary>
        internal static void IdStartAt<TEntity>(this ModelBuilder modelBuilder, int idStartValue)
            where TEntity : BaseEntity
        {
            var seqName = $"{typeof(TEntity).Name}_seq";

            modelBuilder.HasSequence<int>(seqName)
                .StartsAt(idStartValue)
                .IncrementsBy(1);

            modelBuilder.Entity<TEntity>()
                .Property(p => p.Id)
                .HasDefaultValueSql($"nextval('\"{seqName}\"')");
        }
    }
}
