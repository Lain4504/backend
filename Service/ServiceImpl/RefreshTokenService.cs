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

        public async Task<DateTime> GenerateRefreshToken(User user, string refreshToken)
        {
            var existingToken = await _refreshTokenRepository.GetByToken(refreshToken);
            DateTime expirationDate;

            if (existingToken != null)
            {
                existingToken.Token = refreshToken;
                expirationDate = DateTime.UtcNow.AddMinutes(3);
                existingToken.ExpirationDate = expirationDate;
            }
            else
            {
                expirationDate = DateTime.UtcNow.AddMinutes(3);
                var newToken = new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpirationDate = expirationDate,
                    CreatedDate = DateTime.Now // Thời gian tạo mới
                };
                await _refreshTokenRepository.Add(newToken);
            }

            await _refreshTokenRepository.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

            return expirationDate; // Trả về ExpirationDate
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
