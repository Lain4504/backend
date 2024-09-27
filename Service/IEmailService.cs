﻿namespace BackEnd.Service
{
    public interface IEmailService
    {
        Task SendActivationEmail(string email, string token);
        Task SendResetPasswordEmail(string email);
    }
}