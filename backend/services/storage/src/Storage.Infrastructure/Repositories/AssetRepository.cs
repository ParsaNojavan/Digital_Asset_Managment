using Assests.Domain.Entities;
using Dapper;
using Folders.Domain.Entities;
using Storage.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Infrastructure.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly IDbConnection _db;

        public AssetRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task DeleteAsync(Guid id)
        {
            var sql = "DELETE FROM Assets WHERE Id = @Id";
            await _db.ExecuteAsync(sql,new {Id = id});
        }

        public async Task<IEnumerable<Asset>> GetByFolderIdAsync(Guid folderId)
        {
            var sql = @"SELECT * FROM Assets WHERE FolderId = @FolderId";
            return await _db.QueryAsync<Asset>(sql, new {FolderId = folderId});
        }

        public async Task<Asset?> GetByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM Assets WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Asset>(sql, new {Id = id});
        }

        public async Task InsertAsync(Asset asset)
        {
            var sql = @"INSERT INTO Assets (Id, FolderId, FileName, OriginalFileName, ContentType, Size, StorageProvider, StoragePath, CreatedAt, UpdatedAt)
                        VALUES (@Id, @FolderId, @FileName, @OriginalFileName, @ContentType, @Size, @StorageProvider, @StoragePath, @CreatedAt, @UpdatedAt)";

            await _db.ExecuteAsync(sql,asset);
        }

        public async Task UpdateAsync(Asset asset)
        {
            var sql = @"UPDATE Assets
                        SET FileName = @FileName,
                            OriginalFileName = @OriginalFileName,
                            ContentType = @ContentType,
                            Size = @Size,
                            StorageProvider = @StorageProvider,
                            StoragePath = @StoragePath,
                            UpdatedAt = @UpdatedAt
                        WHERE Id = @Id";
            await _db.ExecuteAsync(sql,asset);
        }
    }
}
