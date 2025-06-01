using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.DTOs.Products;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.InvoiceFileRepository;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly private IProductReadRepository _productReadRepository;
        readonly private IProductWriteRepository _productWriteRepository;
        readonly private IWebHostEnvironment _webHostEnvironment;
        readonly private IFileWriteRepository _fileWriteRepository;
        readonly private IFileReadRepository _fileReadRepository;
        readonly private IProductImageFileReadRepository _productImageFileReadRepository;
        readonly private IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly private IInvoiceFileReadRepository _invoiceFileReadRepository;
        readonly private IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        readonly private IStorageService _storageService;


        public ProductController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment, IInvoiceFileWriteRepository ınvoiceFileWriteRepository, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IStorageService storageService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
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

            var pagedResponse = await query.ToPagedResponseAsync(pagination);

            return Ok(pagedResponse);
        }

            [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {

            return Ok(await _productReadRepository.GetByIdAsync(id,false));
        }

        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {

            await _productWriteRepository.AddAsync(
               new()
               {
                   Name = model.Name,
                   Price = model.Price,
                   Stock = model.Stock,
               });
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Price = model.Price;
            product.Name = model.Name;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok(new
            {
                message = "Silme işlemi başarılı"
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(IFormFileCollection formFile, string id)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", formFile);


              Product product = await _productReadRepository.GetByIdAsync(id);

            //foreach(var r in result)
            //{
            //    product.ProductImageFiles.Add(new()
            //    {
            //        FileName = r.fileName,
            //        Path = r.pathOrContainerName,
            //        Storage = _storageService.StorageName,
            //        Products = new List<Product>() { product }
            //    });
            //}

            _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product }
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();


            var d1 = _fileReadRepository.GetAll(false);
            var d2 = _productImageFileReadRepository.GetAll(false);
            var d3 = _invoiceFileReadRepository.GetAll(false);

            return Ok();

        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(string id)
        {
           Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));

            return Ok(product.ProductImageFiles.Select(p => new
            {
                p.Path,
                p.FileName
            }));
        }

        [HttpDelete("[action]/{productId}/{imageId}")]
        public async Task<IActionResult> DeleteProductImage(string productId, string imageId)
        {
            IQueryable<Product> query = _productReadRepository.Table
                .Include(p => p.ProductImageFiles)
                .Where(p => p.Id == Guid.Parse(productId));

            var productImageFile = await query
                .SelectMany(p => p.ProductImageFiles)
                .Where(p => p.Id == Guid.Parse(imageId))
                .FirstOrDefaultAsync();

            query.FirstOrDefault().ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();

            //        var query = _productReadRepository.Table
            //.Include(p => p.ProductImageFiles)
            //.Where(p => p.Id == Guid.Parse(productId));



            //        ProductImageFile? productImageFile = await _productImageFileReadRepository.Table
            //.FirstOrDefaultAsync(p => p.Id == Guid.Parse(imageId) &&
            //                          p.Products.Any(prod => prod.Id == product.Id));

            //ProductImageFile? productImageFile =  product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));

            return Ok();
        }

    }
}
