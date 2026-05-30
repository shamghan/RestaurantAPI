using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Storage
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
        public string? GetBlobSasUrl(string? blobUrl)
        {
            if (string.IsNullOrWhiteSpace(blobUrl))
                return null;

            var blobName = GetBlobNameFromUrl(blobUrl);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _blobStorageSettings.LogosContainerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),

                // Important for Azurite
                Version = "2023-01-03"
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var blobServiceClient =
                new BlobServiceClient(_blobStorageSettings.ConnectionString);

            var credential = new StorageSharedKeyCredential(
                blobServiceClient.AccountName,
                _blobStorageSettings.AccountKey);

            var sasToken = sasBuilder
                .ToSasQueryParameters(credential)
                .ToString();

            return $"{blobUrl}?{sasToken}";
            // blob: https://restaurantssadev.blob.core.windows.net/ logos/ logo-fun.jfif
            // sas: sp=r&st=2024-02-19T08:18:05Z&se=2024-02-19T16:18:05Z&spr=https&sv=2022-11-02&sr=b&sig=bB2hSZtqsbImIuwM7CYMTYSXMrEt5u5K6RJ1EbjrxGA%3D
        }

        private string GetBlobNameFromUrl(string blobUrl)
        {
            var uri = new Uri(blobUrl);

            // Remove container path
            // /devstoreaccount1/logos/myfile.jpg

            var segments = uri.AbsolutePath
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Skip(2); // skip account + container

            return Uri.UnescapeDataString(string.Join("/", segments));
        }
    }
}
