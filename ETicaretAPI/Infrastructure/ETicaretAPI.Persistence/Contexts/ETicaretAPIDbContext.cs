using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Domain.Entities.Identity.AppUsers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Contexts
{
    public class ETicaretAPIDbContext : IdentityDbContext<AppUser,AppRole, string>
    {
        public ETicaretAPIDbContext(DbContextOptions options) : base(options)  { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Domain.Entities.File> Files { get; set; }

        public DbSet<InvoiceFile> Invoices { get; set; }

        public DbSet<ProductImageFile> ProductImages { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var tracktedData = ChangeTracker.Entries<BaseEntity>();

            foreach (var trackedEntityEntry in tracktedData)
            {
                switch (trackedEntityEntry.State)
                {

                    case EntityState.Modified:
                        trackedEntityEntry.Entity.UpdatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        trackedEntityEntry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }


            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
