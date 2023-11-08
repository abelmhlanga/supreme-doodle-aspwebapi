using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicApi.Helpers
{
    public static class FileHelper
    {
        public static async Task<string> UploadImage(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicstoreaccount;AccountKey=6QyalmRBt0c7kLYCfyzBf4W6ehy+UffIphd5KdLsfGN0qmvo+eITerJCV+2NhxtscKSHxdZdSBRX+AStgd759A==;EndpointSuffix=core.windows.net";
            string containerName = "songcover";
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            return  blobClient.Uri.AbsoluteUri;
        }
        public static async Task<string> UploadFile(IFormFile file)
        {
            string connectionString = @"DefaultEndpointsProtocol=https;AccountName=musicstoreaccount;AccountKey=6QyalmRBt0c7kLYCfyzBf4W6ehy+UffIphd5KdLsfGN0qmvo+eITerJCV+2NhxtscKSHxdZdSBRX+AStgd759A==;EndpointSuffix=core.windows.net";
            string containerName = "audiofiles";
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            return  blobClient.Uri.AbsoluteUri;
        }
    }
}