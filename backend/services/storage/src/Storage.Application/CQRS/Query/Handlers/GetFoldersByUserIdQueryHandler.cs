using Folders.Domain.Entities;
using MediatR;
using Storage.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.CQRS.Query.Handlers
{
    public class GetFoldersByUserIdQueryHandler : IRequestHandler<GetFoldersByUserIdQuery, IEnumerable<Folder>>
    {
        private readonly IFolderRepository _folderRepository;

        public GetFoldersByUserIdQueryHandler(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public async Task<IEnumerable<Folder>> Handle(GetFoldersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var folders = (await _folderRepository.GetByUserIdAsync(request.UserId)).ToList();
            return folders;
        }
    }
}
