using BackEnd.Models;
using BackEnd.Repository;
using Microsoft.EntityFrameworkCore;

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

        public async Task GenerateRefreshToken(User user, string refreshToken)
        {
            var existingToken = await _refreshTokenRepository.GetByToken(refreshToken);

            if (existingToken != null)
            {
                existingToken.Token = refreshToken; 
                existingToken.ExpirationDate = DateTime.UtcNow.AddDays(30); 
            }
            else
            {
                var newToken = new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpirationDate = DateTime.UtcNow.AddDays(30),
                    CreatedDate =DateTime.Now// Thời gian hết hạn cho refresh token
                };
                await _refreshTokenRepository.Add(newToken);
            }

            await _refreshTokenRepository.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
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
    }

}
