using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.Responses;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
        this IQueryable<T> query, Pagination pagination)
        {
            var totalCount =  query.Count();
            var data = await query
                .Skip(pagination.Skip)
                .Take(pagination.Size)
                .ToListAsync();

            return new PagedResponse<T>(data, totalCount);
        }
    }
}
