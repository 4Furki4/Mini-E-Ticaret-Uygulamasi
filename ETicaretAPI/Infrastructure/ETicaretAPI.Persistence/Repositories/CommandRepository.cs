using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories
{
    public class CommandRepository<T> : ICommandRepository<T> where T : BaseEntity
    {
        private readonly ETicaretAPIDbContext context;
        public CommandRepository(ETicaretAPIDbContext context)
        {
            this.context = context;
        }
        public DbSet<T> Table => context.Set<T>();

        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(entity);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            await Table.AddRangeAsync(entities);
            return true;
        }

        public bool Remove(T entity)
        {
            EntityEntry entityEntry = Table.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var entity = await Table.FirstOrDefaultAsync(b => b.Id == Guid.Parse(id));
            EntityEntry<T> entityEntry = Table.Remove(entity);
            return entityEntry?.State == EntityState.Deleted;
        }

        public async Task<int> SaveAsync() => await context.SaveChangesAsync();

        public bool Update(T entity)
        {
            EntityEntry<T> entityEntry = Table.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
