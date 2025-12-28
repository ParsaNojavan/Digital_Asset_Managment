using Dapper;
using Sharing.Domain.Entities;
using Sharing.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharing.Infrastructure.Repositories
{
    public class SharedAssetRepository : ISharedAssetRepository
    {
        private readonly IDbConnection _db;

        public SharedAssetRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task DeleteAsync(Guid id)
        {
            var sql = "DELETE FROM SharedAssets WHERE Id = @Id";
            await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<SharedAsset?> GetByIdAsync(Guid id)
        {
            var sql = "SELECT * FROM SharedAssets WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<SharedAsset>(sql,new {Id = id});
        }

        public async Task<IEnumerable<SharedAsset>> GetByOwnerAsync(Guid ownerUserId)
        {
            var sql = "SELECT * FROM SharedAssets WHERE OwnerUserId = @OwnerUserId";
            return await _db.QueryAsync<SharedAsset>(sql, new { OwnerUserId = ownerUserId });
        }

        public async Task<IEnumerable<SharedAsset>> GetByRecipientAsync(Guid sharedWithUserId)
        {
            var sql = "SELECT * FROM SharedAssets WHERE SharedWithUserId = @SharedWithUserId";
            return await _db.QueryAsync<SharedAsset>(sql, new { SharedWithUserId = sharedWithUserId });
        }

        public async Task InsertAsync(SharedAsset sharedAsset)
        {
            var sql = @"INSERT INTO SharedAssets (Id, AssetId, SharedWithUserId, OwnerUserId, CreatedAt)
                        VALUES (@Id, @AssetId, @SharedWithUserId, @OwnerUserId, @CreatedAt)";

            await _db.ExecuteAsync(sql,sharedAsset);
        }

        public async Task UpdateSharedAsset(SharedAsset sharedAsset)
        {
            var sql = @"UPDATE SharedAssets SET 
                               AssetId = @AssetId
                               SharedWithUserId = @SharedWithUserId
                               OwnerUserId = @OwnerUserId
                               WHERE Id = @Id";

            await _db.ExecuteAsync(sql, sharedAsset);
        }
    }
}
