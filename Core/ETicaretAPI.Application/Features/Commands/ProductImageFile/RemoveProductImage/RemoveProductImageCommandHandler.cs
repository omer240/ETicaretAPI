using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage
{



    public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;

        public RemoveProductImageCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
        {

            var query = _productReadRepository.Table.AsQueryable()
                .Include(p => p.ProductImageFiles)
                .Where(p => p.Id == Guid.Parse(request.ProductId));

            var productImageFile = await query
                .SelectMany(p => p.ProductImageFiles)
                .Where(p => p.Id == Guid.Parse(request.ImageId))
                .FirstOrDefaultAsync();

            if(productImageFile != null)
            {
                query.FirstOrDefault()?.ProductImageFiles.Remove(productImageFile);
            }
            
            await _productWriteRepository.SaveAsync();

            return new();
        }
    }
}
