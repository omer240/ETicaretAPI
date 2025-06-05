using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Interfaces
{
    public interface IQueryPagingService
    {
        Task<PagedResponse<T>> PaginateAsync<T>(IQueryable<T> query, Pagination pagination);
    }
}
