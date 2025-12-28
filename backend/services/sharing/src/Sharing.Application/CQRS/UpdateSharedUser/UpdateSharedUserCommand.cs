using MediatR;
using Sharing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.CQRS.UpdateSharedUser
{
    public class UpdateSharedUserCommand : IRequest<Guid>
    {
        public ShareAssetDto shareAssetDto { get; set; }

        public UpdateSharedUserCommand(ShareAssetDto shareAssetDto)
        {
            this.shareAssetDto = shareAssetDto;
        }
    }
}
