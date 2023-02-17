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
    public class AzureStorage : IAzureStorage
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
            return blobContainerClient.GetBlobs().All(blob => blob.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync();
            await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<(string fileName, string pathOrContainerName)> values = new();
            foreach (IFormFile file in files)
            {
                BlobClient blobClient = blobContainerClient.GetBlobClient(file.Name);
                await blobClient.UploadAsync(file.OpenReadStream());
                values.Add((file.Name, containerName));
            }
            return values;
        }
    }
}
