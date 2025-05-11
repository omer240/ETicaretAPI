using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using ETicaretAPI.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Azure
{
    public class AzureStorage : IAzureStorage
    {

        readonly BlobServiceClient _blobServiceClient; //ilgili AzureStorage account'una bağlanmak için kullanılır.
        BlobContainerClient _blobContainerClient; // ilgili account'taki hedef container üzerinde dosya işlemleri yapmak için kullanılır.
        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new(configuration["Storage:Azure"]);

        }
        public Task DeleteAsync(string containerName, string fileName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetFiles(string containerName)
        {
            throw new NotImplementedException();
        }

        public bool HasFile(string containerName, string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<(string fileName, string pathOrContainerName)> datas = new();
            foreach (IFormFile file in files)
            {
               BlobClient blobClient =  _blobContainerClient.GetBlobClient(file.Name);
               await blobClient.UploadAsync(file.OpenReadStream());

            }
        }
    }
}
