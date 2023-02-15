using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Persistence.Repositories
{
    public class FileCommandRepository : CommandRepository<F.File>, IFileCommandRepository
    {
        public FileCommandRepository(ETicaretAPIDbContext context) : base(context) { }
    }
}
