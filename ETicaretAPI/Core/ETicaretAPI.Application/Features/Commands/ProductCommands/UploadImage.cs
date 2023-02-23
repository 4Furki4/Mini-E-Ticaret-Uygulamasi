using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductCommands
{
    public class UploadImageCommandRequest : IRequest<UploadImageCommandResponse>
    {
        public string Id { get; set; }
        public string PathOrContainerName { get; set; }

        public IFormFileCollection FormFiles { get; set; }
        public UploadImageCommandRequest(string id, string pathOrContainerName, IFormFileCollection formFiles)
        {
            Id = id;
            PathOrContainerName = pathOrContainerName;
            FormFiles = formFiles;
        }
    }

    public class UploadImageCommandRequestHandler : IRequestHandler<UploadImageCommandRequest, UploadImageCommandResponse>
    {
        readonly IStorageService storageService;
        readonly IProductImageCommandRepository productImageCommandRepository;
        readonly IProductQueryRepository productQuery;

        public UploadImageCommandRequestHandler(IStorageService storageService, IProductImageCommandRepository productImageCommandRepository, IProductQueryRepository productQuery)
        {
            this.storageService = storageService;
            this.productImageCommandRepository = productImageCommandRepository;
            this.productQuery = productQuery;
        }

        public async Task<UploadImageCommandResponse> Handle(UploadImageCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string pathOrContainerName, string fileName)> values = await storageService.UploadAsync(request.PathOrContainerName, request.FormFiles);
            Product product = await productQuery.GetByIdAsync(request.Id);
            if(product is not null)
            {
                await productImageCommandRepository.AddRangeAsync(values.Select(val => new ProductImageFile()
                {
                    FileName = val.fileName,
                    Path = val.pathOrContainerName,
                    Storage = storageService.StorageType,
                    Products = new List<Product>() { product }
                }).ToList());
                await productImageCommandRepository.SaveAsync();

                return new UploadImageCommandResponse()
                {
                    IsUploaded = true,
                };
            }
            else
            {
                return new UploadImageCommandResponse()
                {
                    IsUploaded = false
                };
            }
            
        }
        
    }

    public class UploadImageCommandResponse
    {
        public bool IsUploaded { get; set; }
    }
}
