using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Concretes
{
    public class ProductService : IProductService
    {
        public List<Product> GetProducts() => new()
        {
            new() { Id = Guid.NewGuid(), Created = DateTime.Now, Name = "Dummy Product 1", Price= 11, Stock = 13 },
            new() { Id = Guid.NewGuid(), Created = DateTime.Now, Name = "Dummy Product 2", Price= 21, Stock = 23 },
            new() { Id = Guid.NewGuid(), Created = DateTime.Now, Name = "Dummy Product 3", Price= 31, Stock = 33 },
            new() { Id = Guid.NewGuid(), Created = DateTime.Now, Name = "Dummy Product 4", Price= 41, Stock = 43 },
            new() { Id = Guid.NewGuid(), Created = DateTime.Now, Name = "Dummy Product 5", Price= 51, Stock = 53 }
        };
    }
}
