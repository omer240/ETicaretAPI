﻿using ETicaretAPI.Application.DTOs.ProductImageFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryResponse
    {
        public List<ProductImage> ProductImages { get; set; }
    }
}
