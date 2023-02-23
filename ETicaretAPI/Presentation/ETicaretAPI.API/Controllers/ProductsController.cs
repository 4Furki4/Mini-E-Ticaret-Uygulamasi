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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static ETicaretAPI.Application.Features.Queries.ProductQueries.GetAllProducts;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductCommandRepository productCommand;
        private readonly IProductQueryRepository productQuery;
        private readonly IValidator<CreateProductViewModel> validator;
        private readonly IWebHostEnvironment webHostEnvironment;
        readonly IProductImageCommandRepository productImageCommandRepository;
        readonly IInvoiceFileCommandRepository invoiceFileCommandRepository;
        readonly IStorageService storageService;
        readonly IConfiguration configuration;


        readonly IMediator mediator;


        public ProductsController(IProductCommandRepository productCommand, IProductQueryRepository productQuery, IValidator<CreateProductViewModel> validator, IWebHostEnvironment webHostEnvironment, IProductImageCommandRepository productImageCommandRepository, IInvoiceFileCommandRepository invoiceFileCommandRepository, IStorageService storageService, IConfiguration configuration, IMediator mediator)
        {
            this.productCommand = productCommand;
            this.productQuery = productQuery;
            this.validator = validator;
            this.webHostEnvironment = webHostEnvironment;
            this.productImageCommandRepository = productImageCommandRepository;
            this.invoiceFileCommandRepository = invoiceFileCommandRepository;
            this.storageService = storageService;
            this.configuration = configuration;
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            GetAllProductsQueryRequest request = new GetAllProductsQueryRequest(pagination);
            GetAllProductsQueryResponse result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(string id) 
        {
            var request = new GetProductByIdQueryRequest(id);
            var result = await mediator.Send(request);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post (CreateProductViewModel viewModel)
        {

            CreateProductCommandRequest request = new CreateProductCommandRequest(viewModel);
            await mediator.Send(request);

            return StatusCode( (int) HttpStatusCode.Created);
        }


        [HttpPut]

        public async Task<IActionResult> Put(PutProductViewModel viewModel)
        {
            UpdateProductCommandRequest request = new UpdateProductCommandRequest(viewModel);
            await mediator.Send(request);
            return Ok(new { message = "Updated!"});
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(string id)
        {
            DeleteProductCommandRequest request = new DeleteProductCommandRequest(id);
            await mediator.Send(request);
            return Ok();
        }

        [HttpPost("[action]")]

        public async Task<IActionResult> Upload(string id)
        {
            IFormFileCollection formFiles = Request.Form.Files;
            UploadImageCommandRequest request = new
                (
                    id: id,
                    pathOrContainerName: "photo-images",
                    formFiles: formFiles
                );
            UploadImageCommandResponse response =  await mediator.Send(request);


            if (response.IsUploaded) return Ok();
            else return BadRequest(new { message = "Product with the given id is not found !"});
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Images(string id)
        {
            GetImageByIdQueryRequest request = new GetImageByIdQueryRequest(id);
            List<GetImageByIdQueryResponse> response =  await mediator.Send(request);
            
            if (response is not null) return Ok(response);
            else return BadRequest(new { message = "Product with the given id is not found !" });
        }

        [HttpDelete("[action]/{Id}")]
        public async Task<IActionResult> Images(string Id, [FromQuery] string imageId)
        {
            Product? product = await productQuery.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(Id));
            if(product is not null)
            {
                ProductImageFile? productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));
                if(productImageFile is not null)
                {
                    product.ProductImageFiles.Remove(productImageFile);
                    await productCommand.SaveAsync();
                    return Ok(new { message = "Image was deleted successfully." });
                }
                else
                {
                    return NotFound("Product found but its image not found");
                }
            }
            else
            {
                return NotFound("Product not found");
            }
            
        }

    }
}
