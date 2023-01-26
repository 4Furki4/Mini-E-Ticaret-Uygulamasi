using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Repositories
{
    public class QueryRepository<T> : IQueryRepository<T> where T : BaseEntity
    {
        private readonly ETicaretAPIDbContext context;
        public QueryRepository(ETicaretAPIDbContext context)
        {
            this.context = context;
        }
        public DbSet<T> Table =>

            context.Set<T>();


        public IQueryable<T> GetAll() => Table;

        

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression) =>
            await Table.FirstOrDefaultAsync(expression);

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression) => Table.Where(expression);
        public async Task<T> GetByIdAsync(string id) =>
            await Table.FirstOrDefaultAsync(b => b.Id == Guid.Parse(id));
    }
}
