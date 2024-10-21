using BackEnd.Models;

namespace BackEnd.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByToken(string token);
        Task Add(RefreshToken refreshToken);
        Task Remove(RefreshToken refreshToken);
        Task SaveChangesAsync();
    }

}
