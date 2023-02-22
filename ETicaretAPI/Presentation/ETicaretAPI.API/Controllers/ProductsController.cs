﻿using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Features.Queries.GetAllProducts;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParams;
using ETicaretAPI.Application.ViewModels.Product;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Infrastructure.Services;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO.Pipelines;

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

            var product = await productQuery.GetByIdAsync(id, false);
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Post (CreateProductViewModel viewModel)
        {
            ValidationResult validationResult = await validator.ValidateAsync(viewModel);
            
            if (ModelState.IsValid)
            {

            }
            Product product = new()
            {
                Stock = viewModel.Stock,
                Price = (long)viewModel.Price,
                Name = viewModel.Name,
            };

            var isCreated = await productCommand.AddAsync(product);

            await productCommand.SaveAsync();
            return CreatedAtAction(nameof(Post), product); 
        }


        [HttpPut]

        public async Task<IActionResult> Put(PutProductViewModel viewModel)
        {
            Product product = await productQuery.GetByIdAsync(viewModel.Id);

            product.Stock = viewModel.Stock;

            product.Price = (long) viewModel.Price;

            product.Name = viewModel.Name;

            await productCommand.SaveAsync();
            return Ok(new { message = "Updated!"});
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(string id)
        {
            bool isDeleted = await productCommand.RemoveAsync(id);
            await productCommand.SaveAsync();
            if (isDeleted)
                return Ok(new {message = "Deleted" });
            else
                return NotFound(new { message = "The product with the given id doesn't exist." });
        }

        [HttpPost("[action]")]

        public async Task<IActionResult> Upload(string id)
        {

            Product product = await productQuery.GetByIdAsync(id);
            
            var values = await storageService.UploadAsync("photo-images", Request.Form.Files);
            await productImageCommandRepository.AddRangeAsync(values.Select(val => new ProductImageFile()
            {
                FileName = val.fileName,
                Path = val.pathOrContainerName,
                Storage = storageService.StorageType,
                Products = new List<Product>() { product}
            }).ToList());
            await productImageCommandRepository.SaveAsync();
            return Ok();
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Images(string id)
        {
            Product? product = await productQuery.Table.AsNoTracking().Include(t => t.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            if (product is not null)
                return Ok(product.ProductImageFiles.Select(p => new
                {
                    path = $"{configuration["BaseStorageUrl"]}/{p.Path}",
                    fileName = p.FileName,
                    id = p.Id.ToString()
                }));
            else
                return Ok();
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
