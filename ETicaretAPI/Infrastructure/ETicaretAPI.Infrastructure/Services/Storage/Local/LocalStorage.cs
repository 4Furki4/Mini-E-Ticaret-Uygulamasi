using ETicaretAPI.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        readonly IWebHostEnvironment webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task DeleteAsync(string fileName, string path)
            => File.Delete($"{path}\\{fileName}");

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directoryInfo = new(path);
            return directoryInfo.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFiles(string fileName, string path) =>
            File.Exists($"{path}\\{fileName}");
        async Task<bool> CopyAsync(IFormFile file, string path)
        {
            try
            {
                using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception)
            {
                //todo custom exception will be created to handle exceptions thrown here.
                return false;
            }
        }
        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        {
            string pathToUpload = Path.Combine(webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(pathToUpload))
                Directory.CreateDirectory(pathToUpload);
            List<(string path, string name)> data = new();
            foreach (IFormFile file in files)
            {
                //string fileNewName = await FileRenameAsync(pathToUpload, file.FileName);
                await CopyAsync(file, $"{pathToUpload}\\{file.Name}");
                data.Add(($"{path}\\{file.Name}", file.Name));
            }
            return data;
        }
    }
}
