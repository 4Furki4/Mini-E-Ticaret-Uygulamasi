﻿using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Persistence.Repositories
{
    public class FileQueryRepository : QueryRepository<F.File>, IFileQueryRepository
    {
        public FileQueryRepository(ETicaretAPIDbContext context) : base(context) { }
    }
}
