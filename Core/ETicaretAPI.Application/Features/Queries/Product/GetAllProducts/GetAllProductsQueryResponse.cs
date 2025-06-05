using ETicaretAPI.Application.DTOs.Products;
using ETicaretAPI.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProducts
{
    public class GetAllProductsQueryResponse
    {
        public int TotalCount { get; set; }
        public List<GetProductsDto> Data { get; set; }
    }
}
