using Sharing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Domain.Repositories
{
    public interface ISharedAssetRepository
    {
        Task InsertAsync(SharedAsset sharedAsset);
        Task<IEnumerable<SharedAsset>> GetByOwnerAsync(Guid ownerUserId);
        Task<IEnumerable<SharedAsset>> GetByRecipientAsync(Guid sharedWithUserId);
        Task UpdateSharedAsset(SharedAsset sharedAsset);  
        Task<SharedAsset?> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
