using ETicaretAPI.Application.DTOs.Products;
using ETicaretAPI.Application.Interfaces;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IQueryPagingService _queryPagingService;

        public GetAllProductsQueryHandler(IProductReadRepository productReadRepository, IQueryPagingService queryPagingService)
        {
            _productReadRepository = productReadRepository;
            _queryPagingService = queryPagingService;
        }

        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _productReadRepository.GetAll(false)
              .Select(p => new GetProductsDto
              {
                  Id = p.Id,
                  Name = p.Name,
                  Stock = p.Stock,
                  Price = p.Price,
                  CreatedDate = p.CreatedDate,
                  UpdateDate = p.UpdateDate
              });

            var pagedResult = await _queryPagingService.PaginateAsync(query, request.pagination);

            return new GetAllProductsQueryResponse
            {
                TotalCount = pagedResult.TotalCount,
                Data = pagedResult.Data
            };
        }
    }
}
