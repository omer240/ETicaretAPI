using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
        this IQueryable<T> query, Pagination pagination)
        {
            var totalCount = await query.CountAsync();
            var data = await query
                .Skip(pagination.Skip)
                .Take(pagination.Size)
                .ToListAsync();

            return new PagedResponse<T>(data, totalCount);
        }
    }
}
