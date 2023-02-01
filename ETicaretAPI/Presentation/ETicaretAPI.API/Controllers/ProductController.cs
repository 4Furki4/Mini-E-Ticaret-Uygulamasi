using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.ViewModels.Product;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductCommandRepository productCommand;
        private readonly IProductQueryRepository productQuery;
        public ProductController(IProductCommandRepository productCommand, IProductQueryRepository productQuery)
        {
            this.productCommand = productCommand;
            this.productQuery = productQuery;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(productQuery.GetAll(false));
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
            return Ok("Updated!");
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(string id)
        {
            bool isDeleted = await productCommand.RemoveAsync(id);
            await productCommand.SaveAsync();
            if (isDeleted)
                return Ok("Deleted");
            else
                return NotFound("The product with the given id doesn't exist.");
        }

    }
}
