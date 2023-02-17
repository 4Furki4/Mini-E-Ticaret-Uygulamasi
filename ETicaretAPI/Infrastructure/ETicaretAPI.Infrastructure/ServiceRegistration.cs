using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Infrastructure.Enums;
using ETicaretAPI.Infrastructure.Services;
using ETicaretAPI.Infrastructure.Services.Storage;
using ETicaretAPI.Infrastructure.Services.Storage.Azure;
using ETicaretAPI.Infrastructure.Services.Storage.Local;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
        }

        public static void AddStorage<T>(this IServiceCollection services) where T : class, IStorage
        {
            services.AddScoped<IStorage, T>();
        }
        public static void AddStorage(this IServiceCollection services, StorageEnum storageEnum)
        {
            switch (storageEnum)
            {
                case StorageEnum.Local:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageEnum.Azure:
                    services.AddScoped<IStorage, AzureStorage>();
                    break;
                //case StorageEnum.AWS:
                //    services.AddScoped<IStorage, AWSStorage>();
                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break; 
            }
        }
    }
}
