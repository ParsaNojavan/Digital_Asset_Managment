using SharedKernel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Domain.Entities
{
    public class SharedAsset : BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AssetId { get; set; }     
        public Guid SharedWithUserId { get; set; }
        public Guid OwnerUserId { get; set; }
    }

}
