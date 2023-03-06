using ETicaretAPI.Application.ViewModels.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<CreateProductViewModel >
    {
        public CreateProductValidator()
        {
            RuleFor(cvm => cvm.Name)
                .NotNull()
                .NotEmpty().WithMessage("Lütfen bir isim giriniz.")
                .MinimumLength(1)
                .MaximumLength(150).WithMessage("İsim uzunluğu 1 ile 150 arasında olmalıdır.");

            RuleFor(cvm => cvm.Price)
                .NotEmpty()
                .NotNull().WithMessage("Lütfen bir fiyat giriniz.")
                .GreaterThanOrEqualTo(0).WithMessage("Fiyat bilgisi sıfırdan büyük olmalıdır!");
            RuleFor(cvm => cvm.Stock)
                .NotEmpty()
                .NotNull().WithMessage("Lütfen bir stok adedi giriniz.")
                .Must(val => val >= 0).WithMessage("Stok bilgisi sıfırdan büyük olmalıdır!");
        }
    }
}
