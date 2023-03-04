
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Services.Authentications;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities.Identity.AppUsers;
using ETicaretAPI.Persistence.Contexts;
using ETicaretAPI.Persistence.Repositories;
using ETicaretAPI.Persistence.Services;
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
            });
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<ETicaretAPIDbContext>();
            services.AddScoped<ICustomerCommandRepository, CustomerCommandRepository>();
            services.AddScoped<ICustomerQueryRepository, CustomerQueryRepository>();
            services.AddScoped<IOrderCommandRepository, OrderCommandRepository>();
            services.AddScoped<IOrderQueryRepository, OrderQueryRepository>();
            services.AddScoped<IProductCommandRepository, ProductCommandRepository>();
            services.AddScoped<IProductQueryRepository, ProductQueryRepository>();
            services.AddScoped<IFileQueryRepository, FileQueryRepository>();
            services.AddScoped<IFileCommandRepository, FileCommandRepository>();
            services.AddScoped<IInvoiceFileCommandRepository, InvoiceFileCommandRepository>();
            services.AddScoped<IInvoiceFileQueryRepository, InvoiceFileQueryRepository>();
            services.AddScoped<IProductImageQueryRepository, ProductImageFileQueryRepository>();
            services.AddScoped<IProductImageCommandRepository, ProductImageCommandRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddHttpClient();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExternalAuth, AuthService>(); // It resolves just external auth interface's members.
            services.AddScoped<IInternalAuth, AuthService>();
        }
    }
}
