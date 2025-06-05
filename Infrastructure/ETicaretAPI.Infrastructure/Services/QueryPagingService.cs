using ETicaretAPI.Application.Interfaces;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class QueryPagingService : IQueryPagingService
    {
        public async Task<PagedResponse<T>> PaginateAsync<T>(IQueryable<T> query, Pagination pagination)
        {
            var totalCount = await query.CountAsync().ConfigureAwait(false);
            var data = await query
                .Skip(pagination.Skip)
                .Take(pagination.Size)
                .ToListAsync();

            return new PagedResponse<T>(data, totalCount);
        }
    }
}
