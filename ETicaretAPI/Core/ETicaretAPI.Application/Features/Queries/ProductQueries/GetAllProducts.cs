using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParams;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductQueries
{
    public class GetAllProducts
    {
        public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
        {
            readonly IProductQueryRepository productQuery;
            public GetAllProductsQueryHandler(IProductQueryRepository productQuery)
            {
                this.productQuery = productQuery;
            }
            public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
            {
                var totalSize = productQuery.GetAll().Count();
                var products = await productQuery.GetAll().Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.CreatedDate,
                    p.UpdatedDate
                }).Skip(request.Pagination.Page * request.Pagination.Size).Take(request.Pagination.Size).ToListAsync();
                return new()
                {
                    Products = products,
                    TotalSize = totalSize
                };
            }
        }
        public class GetAllProductsQueryRequest : IRequest<GetAllProductsQueryResponse>
        {
            public Pagination Pagination { get; set; }

            public GetAllProductsQueryRequest(Pagination pagination)
            {
                Pagination = pagination;
            }
        }

        public class GetAllProductsQueryResponse
        {
            public object Products { get; set; }
            public int TotalSize { get; set; }
        }
    }
}
