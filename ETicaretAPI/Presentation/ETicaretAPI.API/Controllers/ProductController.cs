using ETicaretAPI.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductCommandRepository productCommand;
        public ProductController(IProductCommandRepository productCommand)
        {
            this.productCommand = productCommand;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await productCommand.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                Name = "Test",
                Price = 31,
                Stock = 31
            });
            await productCommand.SaveAsync();
            return Ok();
        }
    }
}
