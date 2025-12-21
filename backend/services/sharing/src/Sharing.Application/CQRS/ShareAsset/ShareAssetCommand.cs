using MediatR;
using Sharing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.ShareAsset
{
    public class ShareAssetCommand : IRequest<Guid>
    {
        public ShareAssetDto ShareDto { get; }

        public ShareAssetCommand(ShareAssetDto shareDto)
        {
            ShareDto = shareDto;
        }
    }
}


