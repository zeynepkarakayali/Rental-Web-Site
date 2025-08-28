using Microsoft.AspNetCore.Identity;
using System.Data;
using Dapper;

namespace aracKiralamaDeneme.Areas.IdentityStores
{
    public class DapperRoleStore : IRoleStore<IdentityRole>
    {
        private readonly IDbConnection _db;

        public DapperRoleStore(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            var sql = @"INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) 
                        VALUES (@Id, @Name, @NormalizedName, @ConcurrencyStamp)";
            await _db.ExecuteAsync(sql, role);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            var sql = "DELETE FROM AspNetRoles WHERE Id=@Id";
            await _db.ExecuteAsync(sql, new { role.Id });
            return IdentityResult.Success;
        }

        public async Task<IdentityRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var sql = "SELECT * FROM AspNetRoles WHERE Id=@Id";
            return await _db.QuerySingleOrDefaultAsync<IdentityRole>(sql, new { Id = roleId });
        }

        public async Task<IdentityRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var sql = "SELECT * FROM AspNetRoles WHERE NormalizedName=@Name";
            return await _db.QuerySingleOrDefaultAsync<IdentityRole>(sql, new { Name = normalizedRoleName });
        }

        public Task<string?> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.NormalizedName);

        public Task<string?> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.Id);

        public Task<string?> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.Name);

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(IdentityRole role, string? roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            var sql = @"UPDATE AspNetRoles 
                        SET Name=@Name, NormalizedName=@NormalizedName, ConcurrencyStamp=@ConcurrencyStamp
                        WHERE Id=@Id";
            await _db.ExecuteAsync(sql, role);
            return IdentityResult.Success;
        }

        public void Dispose() { }
    }
}
