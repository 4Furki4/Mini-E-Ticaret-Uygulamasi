using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Features.Commands.ProductCommands;
using ETicaretAPI.Application.Features.Commands.ProductImageFileCommands;
using ETicaretAPI.Application.Features.Queries.ProductImageFileQueries;
using ETicaretAPI.Application.Features.Queries.ProductQueries;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParams;
using ETicaretAPI.Application.ViewModels.Product;
using ETicaretAPI.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static ETicaretAPI.Application.Features.Queries.ProductQueries.GetAllProducts;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ProductsController : ControllerBase
    {
        readonly IMediator mediator;
        public ProductsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            GetAllProductsQueryRequest request = new(pagination);
            GetAllProductsQueryResponse result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id) 
        {
            GetProductByIdQueryRequest request = new(id);
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post (CreateProductViewModel viewModel)
        {

            CreateProductCommandRequest request = new(viewModel);
            await mediator.Send(request);
            return StatusCode( (int) HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put(PutProductViewModel viewModel)
        {
            UpdateProductCommandRequest request = new(viewModel);
            await mediator.Send(request);
            return Ok(new { message = "Updated!"});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            DeleteProductCommandRequest request = new(id);
            await mediator.Send(request);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(string id)
        {
            IFormFileCollection formFiles = Request.Form.Files;
            UploadImageCommandRequest request = new(id,"photo-images",formFiles);
            UploadImageCommandResponse response =  await mediator.Send(request);

            if (response.IsUploaded)
                return Ok();

            else
                return BadRequest(new { message = "Product with the given id is not found !"});
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Images(string id)
        {
            GetImageByIdQueryRequest request = new GetImageByIdQueryRequest(id);
            List<GetImageByIdQueryResponse> response =  await mediator.Send(request);
            
            if (response is not null)
                return Ok(response);

            else 

                return BadRequest(new { message = "Product with the given id is not found !" });
        }

        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> Images(string Id, [FromQuery] string imageId)
        {
            DeleteImageCommandRequest request = new(Id, imageId);
            DeleteImageCommandResponse response = await mediator.Send(request);

            if (response.HasProduct && response.HasProductImage) 
                return Ok();

            else if (response.HasProduct!) 
                return BadRequest(new { message = "Product with the given id is not found !" });

            else 
                return BadRequest(new { message = "ProductImage with the given id is not found !" });
        }

    }
}
