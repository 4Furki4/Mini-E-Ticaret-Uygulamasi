using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.ViewModels.Product;
using ETicaretAPI.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductCommands
{
    public class CreateProductCommandRequest : IRequest<Unit>
    {
        public CreateProductViewModel CreateProductViewModel { get; set; }

        public CreateProductCommandRequest(CreateProductViewModel createProductViewModel)
        {
            CreateProductViewModel = createProductViewModel;
        }
    }
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, Unit>
    {
        readonly IProductCommandRepository productCommand;
        private readonly IValidator<CreateProductViewModel> validator;
        public CreateProductCommandHandler(IProductCommandRepository productCommand, IValidator<CreateProductViewModel> validator)
        {
            this.productCommand = productCommand;
            this.validator = validator;
        }
        public async Task<Unit> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await validator.ValidateAsync(request.CreateProductViewModel);
            Product product = new()
            {
                Name = request.CreateProductViewModel.Name,
                Price = (long)request.CreateProductViewModel.Price,
                Stock = request.CreateProductViewModel.Stock,
            };
            await productCommand.AddAsync(product);
            await productCommand.SaveAsync();
            return await Unit.Task;
        }
    }
}
