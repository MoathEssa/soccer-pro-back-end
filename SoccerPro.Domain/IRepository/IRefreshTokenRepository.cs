using SoccerPro.Domain.Entities;

namespace SoccerPro.Domain.IRepository
{
    public interface IRefreshTokenRepository
    {
        public Task<bool> AddRefreshTokenAsync(RefreshToken refreshToken);
        public Task<(User? user, bool result)> CheckRefreshTokenIsValidAsync(string refreshToken);

    }
}
