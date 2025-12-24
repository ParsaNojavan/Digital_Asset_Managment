using Folders.Domain.Entities;
using MediatR;
using Storage.Application.CQRS.Command.Folders.UploadFolderCommand;
using Storage.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Handlers.FolderHandlers
{
    public class CreateFolderCommandHandler : IRequestHandler<CreateFolderCommand, Guid>
    {
        private readonly IFolderRepository _folderRepository;

        public CreateFolderCommandHandler(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public async Task<Guid> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
        {
            var path = request.ParentFolderId != null
                ? $"{{ParentFolderId}}/{request.Name}" :
                $"/{request.Name}";

            var folder = new Folder
            {
                UserId = request.UserId,
                Name = request.Name,
                ParentFolderId = request.ParentFolderId,
                Path = path
            };

            await _folderRepository.InsertAsync(folder);

            return folder.Id;
        }
    }
}
