using Storage.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Infrastructure.Storage.Local
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _rootPath;
        public LocalFileStorageService()
        {
            _rootPath = Path.Combine(Directory.GetCurrentDirectory(), "storage-data");
            Directory.CreateDirectory(_rootPath);
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string relativePath, CancellationToken cancellationToken = default)
        {
            var fullPath = Path.Combine(_rootPath, relativePath);

            var directory = Path.GetDirectoryName(fullPath);

            if (!Directory.Exists(directory))   Directory.CreateDirectory(directory!);

            await using var file = new FileStream(fullPath, FileMode.Create
                , FileAccess.Write, FileShare.None);

            await fileStream.CopyToAsync(file, cancellationToken);

            return relativePath;
        }


        public async Task<Stream> DownloadAsync(string relativePath, CancellationToken cancellationToken = default)
        {
            var fullPath = Path.Combine(_rootPath, relativePath); 
            fullPath = Path.GetFullPath(fullPath); 

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("File not found", fullPath);

            return new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }



        public async Task DeleteAsync(string relativePath, CancellationToken cancellationToken = default)
        {
            var fullPath = Path.Combine(_rootPath, relativePath);

            if (File.Exists(fullPath)) File.Delete(fullPath);

            await Task.CompletedTask;
        }
    }
}
