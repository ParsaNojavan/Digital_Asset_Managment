using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;

        public UserRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task InsertUser(User user)
        {
            user.SetCreated();
            var sql = @"
                INSERT INTO Users (Id,UserName,Email,PasswordHash,CreatedAt,UpdatedAt) 
                VALUES (@Id,@UserName,@Email,@PasswordHash,@CreatedAt,@UpdatedAt)";
            await _db.ExecuteAsync(sql, user);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var sql = "SELECT * FROM Users WHERE Email = @Email";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new {Email = email});
        }

        public async Task<User?> GetUserById(Guid id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new {Id = id});
        }
    }
}
