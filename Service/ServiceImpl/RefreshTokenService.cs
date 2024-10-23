using BackEnd.Models;
using BackEnd.Repository;

namespace BackEnd.Service.ServiceImpl
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            return await _refreshTokenRepository.GetByToken(token);
        }

        public async Task GenerateRefreshToken(User user, string token)
        {
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddDays(7) // Thời hạn tùy chỉnh
            };

            await _refreshTokenRepository.Add(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync();
        }

        public async Task RemoveRefreshToken(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetByToken(token);
            if (refreshToken != null)
            {
                await _refreshTokenRepository.Remove(refreshToken);
                await _refreshTokenRepository.SaveChangesAsync();
            }
        }
        public async Task SaveRefreshTokenAsync(long userId, string refreshToken)
        {
            var newRefreshToken = new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                ExpirationDate = DateTime.UtcNow.AddDays(7), // Thiết lập thời hạn sử dụng của refresh token
                CreatedDate = DateTime.UtcNow,
            };

            // Lưu vào cơ sở dữ liệu
            await _refreshTokenRepository.Add(newRefreshToken);
            await _refreshTokenRepository.SaveChangesAsync();
        }

    }

}
