using MediatR;
using Storage.Application.CQRS.Command.Folders.DeleteFolderCommand;
using Storage.Application.Services;
using Storage.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FolderHandlers
{
    public class DeleteFolderCommandHandler : IRequestHandler<DeleteFolderCommand>
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IFileStorageService _storageService;

        public DeleteFolderCommandHandler(IFolderRepository folderRepository, IAssetRepository assetRepository, IFileStorageService storageService)
        {
            _folderRepository = folderRepository;
            _assetRepository = assetRepository;
            _storageService = storageService;
        }
        public async Task Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
        {
            var folder = await _folderRepository.GetByIdAsync(request.FolderId);
            if (folder == null)
                throw new Exception("Folder not found");

            if (folder.UserId != request.UserId)
                throw new Exception("Forbidden");

            var assets = await _assetRepository.GetByFolderIdAsync(folder.Id);

            foreach (var asset in assets)
            {
                await _storageService.DeleteAsync(asset.StoragePath, cancellationToken);
            }

            foreach (var asset in assets)
            {
                await _assetRepository.DeleteAsync(asset.Id);
            }

            await _folderRepository.DeleteAsync(folder.Id);

            await _storageService.DeleteDirectoryAsync(folder.Path, cancellationToken);
        }
    }
}
