using CloudGamesStore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudGamesStore.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task<List<T>> GetPagedAsync<T>(
            this IRepository<T> repository,
            int pageNumber,
            int pageSize,
            Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderByFunc = null)
            where T : class
        {
            // This would need to be implemented in each specific repository
            // as the base repository doesn't expose IQueryable directly
            throw new NotImplementedException("Use specific repository method for paging");
        }

        public static async Task<bool> ExistsWithConditionAsync<T>(
            this IRepository<T> repository,
            Func<T, bool> predicate)
            where T : class
        {
            var all = await repository.GetAllAsync();
            return all.Any(predicate);
        }
    }
}
