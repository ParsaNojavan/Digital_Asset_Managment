using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Application.DTOs
{
    public class ShareAssetDto
    {
        public Guid AssetId { get; set; }
        public Guid SharedWithUserId { get; set; }
        public Guid OwnerUserId { get; set; }
    }
}
