using ETicaretAPI.Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        readonly IStorage storage;

        public StorageService(IStorage storage)
        {
            this.storage = storage;
        }

        public string StorageType => storage.GetType().Name;

        public Task DeleteAsync(string fileName, string pathOrContainerName) => storage.DeleteAsync(fileName, pathOrContainerName);

        public List<string> GetFiles(string pathOrContainerName) => storage.GetFiles(pathOrContainerName);
        

        public bool HasFiles(string fileName, string pathOrContainerName) => storage.HasFiles(fileName, pathOrContainerName);

        public Task<List<(string pathOrContainerName, string fileName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files) 
            => storage.UploadAsync(pathOrContainerName, files);
    }
}
