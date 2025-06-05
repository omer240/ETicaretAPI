using ETicaretAPI.Application.DTOs.ProductImageFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandResponse
    {
        public List<UploadedImageInfo> UploadedImageInfos { get; set; }
    }
}

