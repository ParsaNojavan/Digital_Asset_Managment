using Storage.Application.Services;
using Storage.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Infrastructure.Storage.Local
{
    public class UserStorageCleanupService : IUserStorageCleanupService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IFileStorageService _fileStorageService;

        public UserStorageCleanupService(
            IFolderRepository folderRepository,
            IAssetRepository assetRepository,
            IFileStorageService fileStorageService)
        {
            _folderRepository = folderRepository;
            _assetRepository = assetRepository;
            _fileStorageService = fileStorageService;
        }
        public async Task CleanupAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var folders = (await _folderRepository.GetByUserIdAsync(userId)).ToList();

            foreach (var folder in folders)
            {
                var assets = (await _assetRepository.GetByFolderIdAsync(folder.Id)).ToList();

                foreach (var asset in assets)
                {
                    await _fileStorageService.DeleteAsync(asset.StoragePath, cancellationToken);
                    await _assetRepository.DeleteAsync(asset.Id);
                }
                await _folderRepository.DeleteAsync(folder.Id);
            }

            await _fileStorageService.DeleteDirectoryAsync(userId.ToString(), cancellationToken);
        }
    }
}
