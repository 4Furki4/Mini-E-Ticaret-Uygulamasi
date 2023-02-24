using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
        readonly IProductImageCommandRepository productImageCommand;

        public DeleteImageCommandRequestHandler(IProductQueryRepository productQuery, IProductImageCommandRepository productImageCommand, IProductCommandRepository productCommand)
        {
            this.productQuery = productQuery;
            this.productImageCommand = productImageCommand;
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
