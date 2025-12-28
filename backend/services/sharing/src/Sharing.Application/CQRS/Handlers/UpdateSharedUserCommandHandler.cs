using MediatR;
using Sharing.Application.CQRS.UpdateSharedUser;
using Sharing.Application.DTOs;
using Sharing.Domain.Entities;
using Sharing.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.Handlers
{
    public class UpdateSharedUserCommandHandler : IRequestHandler<UpdateSharedUserCommand, Guid>
    {
        private readonly ISharedAssetRepository _shareRepository;

        public UpdateSharedUserCommandHandler(ISharedAssetRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        public async Task<Guid> Handle(UpdateSharedUserCommand request, CancellationToken cancellationToken)
        {
            var asset = await _shareRepository.GetByIdAsync(request.shareAssetDto.AssetId);

            if (asset == null) throw new KeyNotFoundException("Shared asset not found");

            asset.SharedWithUserId = request.shareAssetDto.SharedWithUserId;

            await _shareRepository.UpdateSharedAsset(asset);

            return request.shareAssetDto.AssetId;
        }
    }
}
