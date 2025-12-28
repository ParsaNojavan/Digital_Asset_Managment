using MediatR;
using Sharing.Domain.Entities;
using Sharing.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.ShareAsset.Handlers
{
    public class ShareAssetCommandHandler : IRequestHandler<ShareAssetCommand, Guid>
    {
        private readonly ISharedAssetRepository _shareRepository;
        public ShareAssetCommandHandler(ISharedAssetRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }
        public async Task<Guid> Handle(ShareAssetCommand request, CancellationToken cancellationToken)
        {
            var sharedAsset = new SharedAsset
            {
                AssetId = request.ShareDto.AssetId,
                SharedWithUserId = request.ShareDto.SharedWithUserId,
                OwnerUserId = request.ShareDto.OwnerUserId
            };

            await _shareRepository.InsertAsync(sharedAsset);
            return sharedAsset.Id;
        }
    }
}
