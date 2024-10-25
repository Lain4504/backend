﻿using BackEnd.Models;

namespace BackEnd.Service
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> GetRefreshToken(string token);
        Task GenerateRefreshToken(User user, string token);
        Task RemoveRefreshToken(string token);
    }
}