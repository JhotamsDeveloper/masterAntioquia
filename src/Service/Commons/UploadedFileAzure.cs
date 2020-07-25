using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Service.Commons
{
    public interface IUploadedFileAzure
    {
        Task<string> SaveFileAzure(IFormFile file, string folderName);
        Task<string> EditFileAzure(IFormFile file, string route, string folderName);
        Task DeleteFile(string route, string folderName);

    }

    public class UploadedFileAzure : IUploadedFileAzure
    {
        private readonly string _configuration;

        public UploadedFileAzure(IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<string> SaveFileAzure(IFormFile file, string folderName) 
        {
            string _uniqueFileName = null;

            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var _content = memoryStream.ToArray();
                    var _ext = Path.GetExtension(file.FileName);

                    _uniqueFileName = await SaveFile(_content, _ext, folderName, file.ContentType);
                }
            }
            return _uniqueFileName;
        }

        public async Task<string> EditFileAzure(IFormFile file, string route, string folderName)
        {
            string _uniqueFileName = null;

            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var _content = memoryStream.ToArray();
                    var _ext = Path.GetExtension(file.FileName);

                    _uniqueFileName = await EditFile(_content, _ext, folderName, route, file.ContentType);
                }
            }
            return _uniqueFileName;
        }


        public async Task DeleteFile(string route, string folderName)
        {
            if (route != null)
            {
                var _accountName = CloudStorageAccount.Parse(_configuration);
                var _client = _accountName.CreateCloudBlobClient();
                var _folderName = _client.GetContainerReference(folderName);

                var _nameBlob = Path.GetFileName(route);
                var _blob = _folderName.GetBlobReference(_nameBlob);
                await _blob.DeleteIfExistsAsync();

            }
        }

        private async Task<string> EditFile(byte[] content, string ext, string folderName, string route, string contentType)
        {
            await DeleteFile(route, folderName);

            return await SaveFile(content, ext, folderName, contentType);
        }

        private async Task<string> SaveFile(byte[] content, string ext, string folderName, string contentType)
        {
            var _accountName = CloudStorageAccount.Parse(_configuration);
            var _client = _accountName.CreateCloudBlobClient();
            var _folderName = _client.GetContainerReference(folderName);

            await _folderName.CreateIfNotExistsAsync();
            await _folderName.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            var _nameFile = $"{Guid.NewGuid()}{ext}";
            var _blob = _folderName.GetBlockBlobReference(_nameFile);

            await _blob.UploadFromByteArrayAsync(content, 0, content.Length);
            _blob.Properties.ContentType = contentType;

            await _blob.SetPropertiesAsync();

            return _blob.Uri.ToString();
        }
    }
}
