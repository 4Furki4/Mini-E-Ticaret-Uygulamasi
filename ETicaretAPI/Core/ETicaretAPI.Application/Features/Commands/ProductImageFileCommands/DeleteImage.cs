using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFileCommands
{
    public class DeleteImageCommandRequest : IRequest<DeleteImageCommandResponse>
    {
        public string Id { get; set; }
        public string ImageId { get; set; }
        public DeleteImageCommandRequest(string id, string imageId)
        {
            Id = id;
            ImageId = imageId;
        }
    }


    public class DeleteImageCommandRequestHandler : IRequestHandler<DeleteImageCommandRequest, DeleteImageCommandResponse>
    {
        readonly IProductQueryRepository productQuery;
        readonly IProductCommandRepository productCommand;

        public DeleteImageCommandRequestHandler(IProductQueryRepository productQuery, IProductCommandRepository productCommand)
        {
            this.productQuery = productQuery;
            this.productCommand = productCommand;
        }

        public async Task<DeleteImageCommandResponse> Handle(DeleteImageCommandRequest request, CancellationToken cancellationToken)
        {
            Product? product = await productQuery.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));
            DeleteImageCommandResponse response = new();
            if (product != null)
            {
                response.HasProduct = true;
                ProductImageFile? productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(request.ImageId));

                if(productImageFile != null)
                {
                    response.HasProductImage = true;
                    product.ProductImageFiles.Remove(productImageFile);
                    await productCommand.SaveAsync();
                }
            }
            return response;
        }
    }
    public class DeleteImageCommandResponse
    {
        public bool HasProduct { get; set; }
        public bool HasProductImage { get; set;}
    }
}
