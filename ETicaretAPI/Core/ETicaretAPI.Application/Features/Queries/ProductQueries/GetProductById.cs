using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductQueries
{
    public class GetProductByIdQueryRequest : IRequest<GetProductByIdQueryResponse>
    {
        public string Id { get; set; }

        public GetProductByIdQueryRequest(string id)
        {
            Id = id;
        }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        readonly IProductQueryRepository productQuery;
        public GetProductByIdQueryHandler(IProductQueryRepository productQuery)
        {
            this.productQuery = productQuery;
        }
        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            Product product = await productQuery.GetByIdAsync(request.Id, false);
            return new GetProductByIdQueryResponse()
            {
                Product = product
            };
        }
    }


    public class GetProductByIdQueryResponse
    {
        public Product? Product { get; set; }
    }
}
