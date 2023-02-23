using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.ViewModels.Product;
using ETicaretAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductCommands
{
    public class UpdateProductCommandRequest : IRequest<Unit>
    {
        public PutProductViewModel PutProductViewModel { get; set; }

        public UpdateProductCommandRequest(PutProductViewModel putProductViewModel)
        {
            PutProductViewModel = putProductViewModel;
        }
    }
    public class UpdateProductCommandRequestHandler : IRequestHandler<UpdateProductCommandRequest, Unit>
    {
        readonly IProductCommandRepository commandRepository;
        readonly IProductQueryRepository queryRepository;

        public UpdateProductCommandRequestHandler(IProductCommandRepository commandRepository, IProductQueryRepository queryRepository)
        {
            this.commandRepository = commandRepository;
            this.queryRepository = queryRepository;
        }

        public async Task<Unit> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            Product product = await queryRepository.GetByIdAsync(request.PutProductViewModel.Id);
            product.Stock = request.PutProductViewModel.Stock;

            product.Price = (long)request.PutProductViewModel.Price;

            product.Name = request.PutProductViewModel.Name;

            await commandRepository.SaveAsync();

            return await Unit.Task;
        }
    }
}
