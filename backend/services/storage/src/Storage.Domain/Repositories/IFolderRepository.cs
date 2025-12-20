using Folders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Domain.Repositories
{
    public interface IFolderRepository
    {
        Task InsertAsync(Folder folder);
        Task <IEnumerable<Folder>> GetByUserIdAsync(Guid userId);
        Task<Folder?> GetByIdAsync(Guid id);
        Task UpdateAsync(Folder folder);
        Task DeleteAsync(Guid id);
    }
}
