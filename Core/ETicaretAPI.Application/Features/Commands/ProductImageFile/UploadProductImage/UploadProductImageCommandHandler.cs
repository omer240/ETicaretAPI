using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.DTOs.ProductImageFiles;
using ETicaretAPI.Application.Repositories;
using MediatR;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IStorageService _storageService;

        readonly IProductReadRepository _productReadRepository;

        public UploadProductImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService, IProductReadRepository productReadRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _storageService = storageService;
            _productReadRepository = productReadRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", request.FormFiles);


            Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);

            _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new Domain.Entities.ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Domain.Entities.Product>() { product }
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();

            return new()
            {
                UploadedImageInfos = result.Select(r => new UploadedImageInfo
                {
                    FileName = r.fileName,
                    PathOrContainerName = r.pathOrContainerName
                }).ToList()
            };

        }
    }
}
