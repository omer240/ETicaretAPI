using Azure.Core;
using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct
{
    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;

        public GetByIdProductQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
        {
            var response = await _productReadRepository.GetByIdAsync(request.Id, false);

            return new()
            {
                Id = response.Id,
                Name = response.Name,
                Stock = response.Stock,
                Price = response.Price,
                CreatedDate = response.CreatedDate,
                UpdateDate = response.UpdateDate,
            };
        }
    }
}
