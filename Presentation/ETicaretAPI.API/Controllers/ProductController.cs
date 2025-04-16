﻿using ETicaretAPI.Application.DTOs.Products;
using ETicaretAPI.Application.Repositories.ProductRepository;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.Responses;
using ETicaretAPI.Application.Services;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Infrastructure.Extensions;
using ETicaretAPI.Persistence.Repositories.ProductRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        readonly private IFileService _fileService;

        public ProductController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
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

            return Ok(_productReadRepository.GetByIdAsync(id,false));
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
        public async Task<IActionResult> Upload()
        {
            _fileService.UploadAsync("resource/product-images", Request.Form.Files);
            return Ok();
         
        }

    }
}
