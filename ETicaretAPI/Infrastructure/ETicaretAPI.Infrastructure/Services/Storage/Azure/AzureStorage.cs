using Azure.Storage.Blobs;
using ETicaretAPI.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;

namespace ETicaretAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
        readonly BlobServiceClient blobServiceClient;
        BlobContainerClient blobContainerClient;

        public AzureStorage(IConfiguration configuration)
        {
            blobServiceClient = new(configuration["Storage:Azure"]);
        }
        public async Task DeleteAsync(string fileName, string containerName)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return blobContainerClient.GetBlobs().Select(blob => blob.Name).ToList();
        }

        public bool HasFiles(string fileName, string containerName)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return blobContainerClient.GetBlobs().Any(blob => blob.Name == fileName);
        }

        public async Task<List<(string pathOrContainerName, string fileName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync();
            await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<(string pathOrContainerName, string fileName)> values = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(containerName, HasFiles, file.Name);
                BlobClient blobClient = blobContainerClient.GetBlobClient(fileNewName);
                await blobClient.UploadAsync(file.OpenReadStream());
                values.Add(($"{containerName}/{fileNewName}",fileNewName));
            }
            return values;
        }
    }
}
