using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFileQueries
{
    public class GetImageByIdQueryRequest : IRequest<List<GetImageByIdQueryResponse>>
    {
        public string Id { get; set; }

        public GetImageByIdQueryRequest(string id)
        {
            Id = id;
        }
    }

    public class GetImageByIdQueryRequestHandler : IRequestHandler<GetImageByIdQueryRequest, List<GetImageByIdQueryResponse>>
    {
        readonly IProductQueryRepository productQuery;
        readonly IConfiguration configuration;

        public GetImageByIdQueryRequestHandler(IProductQueryRepository productQuery, IConfiguration configuration)
        {
            this.productQuery = productQuery;
            this.configuration = configuration;
        }

        public async Task<List<GetImageByIdQueryResponse>> Handle(GetImageByIdQueryRequest request, CancellationToken cancellationToken)
        {
            Product? product = await productQuery.Table.AsNoTracking().Include(t => t.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));
            if(product is not null)
            {
                return product.ProductImageFiles.Select(pif =>
                {
                    return new GetImageByIdQueryResponse()
                    {
                        Id = pif.Id.ToString(),
                        FileName = pif.FileName,
                        Path = $"{configuration["BaseStorageUrl"]}/{pif.Path}",
                    };
                }).ToList();
            }
            else
            {
                return null;
            }

        }
    }

    public class GetImageByIdQueryResponse
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Id { get; set; }
    }
}