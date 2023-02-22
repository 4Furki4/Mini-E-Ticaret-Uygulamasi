using ETicaretAPI.Application.RequestParams;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.GetAllProducts
{
    public class GetAllProductsQueryRequest : IRequest<GetAllProductsQueryResponse>
    {
        public Pagination Pagination { get; set; }

        public GetAllProductsQueryRequest(Pagination pagination)
        {
            Pagination = pagination;
        }
    }
}
