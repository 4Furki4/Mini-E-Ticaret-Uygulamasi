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
        private readonly ICustomerCommandRepository customerCommand;
        private readonly ICustomerQueryRepository customerQuery;
        private readonly IOrderCommandRepository orderCommand;
        private readonly IOrderQueryRepository orderQuery;
        public ProductController(IProductCommandRepository productCommand, IProductQueryRepository productQuery, ICustomerCommandRepository customerCommand, ICustomerQueryRepository customerQuery, IOrderCommandRepository orderCommand, IOrderQueryRepository orderQuery)
        {
            this.productCommand = productCommand;
            this.productQuery = productQuery;
            this.customerCommand = customerCommand;
            this.customerQuery = customerQuery;
            this.orderCommand = orderCommand;
            this.orderQuery = orderQuery;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            bool productHasItems = productQuery.GetAll().Any();
            if (!productHasItems)
            {
                await productCommand.AddAsync(new()
                {
                    Name = "Silgi",
                    Price = 15,
                    Stock = 1000,
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow
                });
                await productCommand.SaveAsync();
            }
            var customerId = Guid.NewGuid();
            await customerCommand.AddAsync(new()
            {
                Name = "Furkan",
                Id = customerId
            });
            await orderCommand.AddAsync(new()
            {
                Address = "Çekmeköy İstanbul",
                Description = "sipariş test",
                Id = Guid.NewGuid(),
                CustomerId = customerId
            });
            await productCommand.SaveAsync();
            var product = productQuery.GetAll();
            return Ok(product);
        }
        [HttpGet("id")]
        public async Task GetById(string id)
        {
            Customer customer= await customerQuery.GetByIdAsync(id);

            customer.Name = "Meltem";

            await productCommand.SaveAsync();
        }


    }
}
