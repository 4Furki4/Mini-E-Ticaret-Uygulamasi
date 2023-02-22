using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.GetAllProducts
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
            var totalSize = productQuery.GetAll(false).Count();
            var products = await productQuery.GetAll(false).Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock,
                p.CreatedDate,
                p.UpdatedDate
            }).Skip(request.Pagination.Size * request.Pagination.Page).Take(request.Pagination.Size).ToListAsync();
            return new() { Products = products, TotalSize = totalSize };
        }
    }
}
