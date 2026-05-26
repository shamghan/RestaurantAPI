using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Repositories
{
    public class BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettingsOption) : IBlobStorageService
    {
        private readonly BlobStorageSettings _blobStorageSettings = blobStorageSettingsOption.Value;
        public async Task<string> UploadToBlobAsync(Stream data, string fileName)
        {
            var blobServiceClient = new BlobServiceClient(_blobStorageSettings.ConnectionString);
            var container =blobServiceClient.GetBlobContainerClient(_blobStorageSettings.LogosContainerName);
            var blobClient = container.GetBlobClient(fileName);
            await blobClient.UploadAsync(data);
            var blobUri = blobClient.Uri.ToString();
            return blobUri;
        }
    }
}
