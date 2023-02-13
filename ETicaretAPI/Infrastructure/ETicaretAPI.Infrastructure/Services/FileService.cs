using ETicaretAPI.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public Task<string> FileRenameAsync(string path)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CopyAsync(IFormFile file, string path)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, false);
                await fileStream.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception)
            {
                //todo custom exception will be created to handle exceptions thrown here.
                return false;
            }
        }

        public async Task<List<(string path, string name)>> UploadAsync(string path, IFormFileCollection files)
        {
            string pathToUpload = Path.Combine(webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(pathToUpload))
                Directory.CreateDirectory(pathToUpload);
            List<bool> results = new();
            List<(string path, string name)> data = new();
            foreach(IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(file.FileName);
                bool result = await CopyAsync(file, $"{pathToUpload}\\${fileNewName}");
                data.Add(($"{pathToUpload}\\${fileNewName}", fileNewName));
                results.Add(result);
            }
            if (results.TrueForAll(result => result.Equals(true)))
                return data;
            else
                return null;
            //todo custom exception will be created and thrown when there is an error while coping data.

        }
    }
}
