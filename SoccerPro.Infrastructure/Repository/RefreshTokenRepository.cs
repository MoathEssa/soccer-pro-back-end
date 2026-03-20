using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SoccerPro.Domain.Entities;
using SoccerPro.Domain.IRepository;
using SoccerPro.Infrastructure.Data;
using System.Data;

namespace SoccerPro.Infrastructure.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private AppDbContext _dbContext;
        private readonly IDbConnection _connection;

        public RefreshTokenRepository(AppDbContext dbContext, IDbConnection connection)
        {
            _dbContext = dbContext;
            _connection = connection;
        }

        public async Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            using var conn = new SqlConnection(_connection.ConnectionString);

            if (_connection.State != ConnectionState.Open)
                await conn.OpenAsync();

            using var command = conn.CreateCommand();
            command.CommandText = "InsertRefreshToken";
            command.CommandType = CommandType.StoredProcedure;

            var sqlParams = new[]
            {
            new SqlParameter("@Token", SqlDbType.NVarChar, 200) { Value = refreshToken.Token },
            new SqlParameter("@UserId", SqlDbType.Int) { Value = refreshToken.UserId },
            new SqlParameter("@ExpiryTime", SqlDbType.DateTime2) { Value = refreshToken.ExpiryTime },
            new SqlParameter("@IsRevoked", SqlDbType.Bit) { Value = refreshToken.IsRevoked },
            new SqlParameter("@CreatedAt", SqlDbType.DateTime2) { Value = refreshToken.CreatedAt },
        };

            foreach (var param in sqlParams)
                command.Parameters.Add(param);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }


        public async Task<(User? user, bool result)> CheckRefreshTokenIsValidAsync(string refreshToken)
        {
            var user = await _dbContext.Users
                .Include(u => u.RefreshTokens)
                .Where(u => u.RefreshTokens.Any(r =>
                    r.Token == refreshToken &&
                    !r.IsRevoked &&
                    r.ExpiryTime > DateTime.UtcNow
                ))
                .FirstOrDefaultAsync();

            return (user, user != null);
        }


    }
}
