using aracKiralamaDeneme.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Dapper;

namespace aracKiralamaDeneme.Areas.IdentityStores
{
    public class DapperUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>
    {
        private readonly IDbConnection _db;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public DapperUserStore(IDbConnection db, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = user.UserName.ToUpperInvariant();
            user.NormalizedEmail = user.Email?.ToUpperInvariant();

            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                // Eğer hash zaten set edilmişse bırak
            }
            else
            {
                var hasher = new PasswordHasher<ApplicationUser>();
                user.PasswordHash = hasher.HashPassword(user, user.PasswordHash);
            }

            var sql = @"INSERT INTO AspNetUsers 
                        (Id, UserName, NormalizedUserName, Email, NormalizedEmail, 
                         EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, 
                         PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
                        VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, 
                                @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, 
                                @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount)";
            await _db.ExecuteAsync(sql, user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var sql = "DELETE FROM AspNetUsers WHERE Id = @Id";
            await _db.ExecuteAsync(sql, new { user.Id });
            return IdentityResult.Success;
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var sql = "SELECT * FROM AspNetUsers WHERE Id = @Id";
            return await _db.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { Id = userId });
        }

        public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var sql = "SELECT * FROM AspNetUsers WHERE NormalizedUserName = @Name";
            return await _db.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { Name = normalizedUserName });
        }

        public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.NormalizedUserName);

        public Task<string?> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.Id);

        public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var sql = @"UPDATE AspNetUsers SET 
                        UserName=@UserName, NormalizedUserName=@NormalizedUserName,
                        Email=@Email, NormalizedEmail=@NormalizedEmail,
                        EmailConfirmed=@EmailConfirmed, PasswordHash=@PasswordHash,
                        SecurityStamp=@SecurityStamp, ConcurrencyStamp=@ConcurrencyStamp,
                        PhoneNumber=@PhoneNumber, PhoneNumberConfirmed=@PhoneNumberConfirmed,
                        TwoFactorEnabled=@TwoFactorEnabled, LockoutEnd=@LockoutEnd,
                        LockoutEnabled=@LockoutEnabled, AccessFailedCount=@AccessFailedCount
                        WHERE Id=@Id";
            await _db.ExecuteAsync(sql, user);
            return IdentityResult.Success;
        }

        // Password işlemleri
        public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash != null);

        public void Dispose() { }



        // IUserEmailStore
        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var sql = "SELECT * FROM AspNetUsers WHERE NormalizedEmail = @NormalizedEmail";
            return await _db.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { NormalizedEmail = normalizedEmail });
        }

        public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }
    }
}
