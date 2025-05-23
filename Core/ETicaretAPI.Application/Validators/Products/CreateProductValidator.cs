﻿using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p=>p.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen ürün adını boş geçmeyiniz")
                .MaximumLength(150)
                    .WithMessage("Lütfen ürün adını 5 ile 150 karakter arasında giriniz.");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen stok bilgisini boş geçmeyiniz.")
                .Must(s => s >= 0)
                    .WithMessage("Stok Bilgisi negatif olamaz");

            RuleFor(p => p.Price)
               .NotEmpty()
               .NotNull()
                   .WithMessage("Lütfen fiyat bilgisini boş geçmeyiniz.")
               .Must(s => s >= 0)
                   .WithMessage("Fiyat Bilgisi negatif olamaz");
        }
    }
}
