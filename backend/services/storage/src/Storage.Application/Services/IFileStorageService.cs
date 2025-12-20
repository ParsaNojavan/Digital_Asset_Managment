using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            string relativePath,
            CancellationToken cancellationToken = default);
        Task<Stream> DownloadAsync(string relativePath,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(string relativePath,
            CancellationToken cancellationToken = default);
    }
}
