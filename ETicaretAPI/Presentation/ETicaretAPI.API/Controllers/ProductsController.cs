using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParams;
using ETicaretAPI.Application.Services;
using ETicaretAPI.Application.ViewModels.Product;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Infrastructure.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IFileService fileService;
        public ProductsController(IProductCommandRepository productCommand, IProductQueryRepository productQuery, IValidator<CreateProductViewModel> validator, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            this.productCommand = productCommand;
            this.productQuery = productQuery;
            this.validator = validator;
            this.webHostEnvironment = webHostEnvironment;
            this.fileService = fileService;
        }
        [HttpGet]
        public IActionResult Get([FromQuery] Pagination pagination)
        {
           var totalSize = productQuery.GetAll(false).Count();
           var products = productQuery.GetAll(false)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.CreatedDate,
                    p.UpdatedDate
                }).Skip(pagination.Size * pagination.Page).Take(pagination.Size).ToList();
            return Ok(new
            {
                products,
                totalSize
            });
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

        public async Task<IActionResult> Upload()
        {
            //Random r = new();
            //string sourcePath = Path.Combine(webHostEnvironment.WebRootPath, "source/product-images");

            //if(!Directory.Exists(sourcePath))
            //    Directory.CreateDirectory(sourcePath);
            //foreach(IFormFile file in Request.Form.Files)
            //{
            //    string fullPath = Path.Combine(sourcePath, $"{r.Next()}{Path.GetExtension(file.FileName)}");
            //    using FileStream fileStream = 
            //        new
            //        (
            //            fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, false
            //        );
            //    await file.CopyToAsync( fileStream );
            //    await fileStream.FlushAsync();
            //}
            var values = await fileService.UploadAsync("source/product-images", Request.Form.Files);
            return Ok(values);

            return Ok();
        }

    }
}
