
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistence.Contexts;
using ETicaretAPI.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<ETicaretAPIDbContext>(opt =>
            {
                opt.UseMySQL(Configuration.ConnectionString);
            }, ServiceLifetime.Singleton);
            services.AddSingleton<ICustomerCommandRepository, CustomerCommandRepository>();
            services.AddSingleton<ICustomerQueryRepository, CustomerQueryRepository>();
            services.AddSingleton<IOrderCommandRepository, OrderCommandRepository>();
            services.AddSingleton<IOrderQueryRepository, OrderQueryRepository>();
            services.AddSingleton<IProductCommandRepository, ProductCommandRepository>();
            services.AddSingleton<IProductQueryRepository, ProductQueryRepository>();
        }
    }
}
