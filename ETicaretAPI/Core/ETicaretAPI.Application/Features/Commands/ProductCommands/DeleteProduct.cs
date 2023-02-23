using ETicaretAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductCommands
{
    public class DeleteProductCommandRequest : IRequest<Unit>
    {
        public string Id { get; set; }

        public DeleteProductCommandRequest(string id)
        {
            Id = id;
        }
    }
    public class DeleteProductCommandRequestHandler : IRequestHandler<DeleteProductCommandRequest, Unit>
    {
        readonly IProductCommandRepository commandRepository;

        public DeleteProductCommandRequestHandler(IProductCommandRepository commandRepository)
        {
            this.commandRepository = commandRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            await commandRepository.RemoveAsync(request.Id);
            await commandRepository.SaveAsync();
            return await Unit.Task;
        }
    }
}
