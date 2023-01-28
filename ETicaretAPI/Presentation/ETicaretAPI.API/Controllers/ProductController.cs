using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
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
            var product = productQuery.GetAll();
            return Ok(product);
        }
        [HttpGet("id")]
        public async Task GetById(string id)
        {
            Product product = await productQuery.GetByIdAsync(id, false);

            product.Name = "Furkan";

            await productCommand.SaveAsync();
        }
    }
}
