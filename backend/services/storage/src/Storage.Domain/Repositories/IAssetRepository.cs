using Assests.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Domain.Repositories
{
    public interface IAssetRepository
    {
        Task InsertAsync(Asset asset);
        Task<IEnumerable<Asset>> GetByFolderIdAsync(Guid folderId);
        Task<Asset?> GetByIdAsync(Guid id);
        Task UpdateAsync(Asset asset);
        Task DeleteAsync(Guid id);
    }
}
