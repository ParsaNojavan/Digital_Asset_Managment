using MediatR;
using Storage.Application.CQRS.Command.Folders.RenameFolderCommand;
using Storage.Application.Services;
using Storage.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FolderHandlers
{
    public class RenameFolderCommandHandler : IRequestHandler<RenameFolderCommand, string>
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IFileStorageService _fileStorageService;

        public RenameFolderCommandHandler(IFolderRepository folderRepository
            , IFileStorageService fileStorageService)
        {
            _folderRepository = folderRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<string> Handle(RenameFolderCommand request, CancellationToken cancellationToken)
        {
            var folder = await _folderRepository.GetByIdAsync(request.FolderId);
            if (folder == null)
                throw new Exception("Folder not found");

            if (folder.UserId != request.UserId)
                throw new Exception("Forbidden");

            string previosDirectory = folder.Path;

            string parentDirectory = Path.GetDirectoryName(folder.Path);

            folder.Name = request.Name;
            folder.Path = parentDirectory + "/" + request.Name;

            await _fileStorageService.MoveDirectoryAsync(previosDirectory,folder.Path,cancellationToken);
            await _folderRepository.UpdateAsync(folder);

            return folder.Path;
        }
    }
}
