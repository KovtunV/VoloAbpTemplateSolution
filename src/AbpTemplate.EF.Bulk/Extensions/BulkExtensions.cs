using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AbpTemplate.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Npgsql.Bulk;

namespace AbpTemplate.EF.Bulk.Extensions
{
    public static class BulkExtensions
    {
        #region Insert

        public static void BulkInsert<TEntity>(this DbContext dbContext, IEnumerable<TEntity> entities)
            where TEntity : BaseEntity
        {
            var bulk = new NpgsqlBulkUploader(dbContext);
            bulk.Insert(entities);
        }

        public static async Task BulkInsertAsync<TEntity>(this DbContext dbContext, IEnumerable<TEntity> entities)
            where TEntity : BaseEntity
        {
            var bulk = new NpgsqlBulkUploader(dbContext);
            await bulk.InsertAsync(entities);
        }

        #endregion

        #region Update

        public static void BulkUpdate<TEntity>(this DbContext dbContext, IEnumerable<TEntity> entities)
            where TEntity : BaseEntity
        {
            var bulk = new NpgsqlBulkUploader(dbContext);
            bulk.Update(entities);
        }

        public static async Task BulkUpdateAsync<TEntity>(this DbContext dbContext, IEnumerable<TEntity> entities)
            where TEntity : BaseEntity
        {
            var bulk = new NpgsqlBulkUploader(dbContext);
            await bulk.UpdateAsync(entities);
        }

        #endregion

        #region Delete

        public static int Delete<TEntity>(this IQueryable<TEntity> query, IEnumerable<TEntity> entities)
            where TEntity : BaseEntity
        {
            if (entities is null || !entities.Any())
            {
                return 0;
            }

            var ids = entities.Select(q => q.Id).ToList();
            return Delete(query.Where(q => ids.Contains(q.Id)));
        }

        public static Task<int> DeleteAsync<TEntity>(this IQueryable<TEntity> query, IEnumerable<TEntity> entities)
            where TEntity : BaseEntity
        {
            if (entities is null || !entities.Any())
            {
                return Task.FromResult(0);
            }

            var ids = entities.Select(q => q.Id).ToList();
            return DeleteAsync(query.Where(q => ids.Contains(q.Id)));
        }

        public static int Delete<TEntity>(this IQueryable<TEntity> query)
            where TEntity : BaseEntity
        {
            return Z.EntityFramework.Plus.BatchDeleteExtensions.Delete(query);
        }

        public static Task<int> DeleteAsync<TEntity>(this IQueryable<TEntity> query)
            where TEntity : BaseEntity
        {
            return Z.EntityFramework.Plus.BatchDeleteExtensions.DeleteAsync(query);
        }

        #endregion
    }
}
