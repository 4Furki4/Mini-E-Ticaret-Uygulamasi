using ETicaretAPI.Application.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        private async Task<string> FileRenameAsync(string path, string fileName)
        {
            return await Task.Run<string>(() =>
            {
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                string extension = Path.GetExtension(fileName);
                string newFileName = $"{NameOperation.NameRegulatory(oldName)}{extension}";
                bool fileIsExists = false;
                int fileIndex = 0;
                do
                {
                    if (File.Exists($"{path}\\{newFileName}"))
                    {
                        fileIsExists = true;
                        fileIndex++;
                        newFileName = $"{NameOperation.NameRegulatory(oldName + "-" + fileIndex)}{extension}";
                    }
                    else
                    {
                        fileIsExists = false;
                    }
                } while (fileIsExists);

                return newFileName;
            });



        }

        public async Task<bool> CopyAsync(IFormFile file, string path)
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

        public async Task<List<(string path, string name)>> UploadAsync(string path, IFormFileCollection files)
        {
            string pathToUpload = Path.Combine(webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(pathToUpload))
                Directory.CreateDirectory(pathToUpload);
            List<bool> results = new();
            List<(string path, string name)> data = new();
            foreach(IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(pathToUpload, file.FileName);
                bool result = await CopyAsync(file, $"{pathToUpload}\\{fileNewName}");
                data.Add(($"{path}\\{fileNewName}", fileNewName));
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
