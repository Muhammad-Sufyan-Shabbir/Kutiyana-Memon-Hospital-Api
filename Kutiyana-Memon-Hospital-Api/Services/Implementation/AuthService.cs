using Kutiyana_Memon_Hospital_Api.API.Data;
using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.API.Helpers;
using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using Kutiyana_Memon_Hospital_Api.API.UnitOfWork.Interfaces;
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;
using Kutiyana_Memon_Hospital_Api.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(ApplicationDbContext context, IJwtService jwtService, IPasswordHasher<User> passwordHasher,IEmailService emailService)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<string?> LoginAsync(AuthRequestDto request)
        {
            // 1. User find karo (email ya username se)
            var user = await _context.ApplicationUser
                .Include(u => u.Role)
                .Include(u => u.company)
                .FirstOrDefaultAsync(u => u.Email == request.EmailOrUsername || u.UserName == request.EmailOrUsername);

            if (user == null) return null;

            // 2. Verify password with helper
            bool isPasswordValid = PasswordHelper.VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt);
            if (!isPasswordValid) return null;
             
            var moduleAccesses = await _context.roleModuleAccess
                .Where(rm => rm.RoleId == user.RoleId)
                .Include(rm => rm.Module)
                .ToListAsync(); 

            var token = _jwtService.GenerateToken(user, user.Role!, moduleAccesses);
             
            return token;
        }
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _context.ApplicationUser.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) throw new Exception("User not found");

            // Generate token & expiry
            user.ResetPasswordToken = Guid.NewGuid().ToString();
            user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);
            await _context.SaveChangesAsync();

            // Create reset link
            string resetLink = $"http://localhost:4200/auth/reset-password?token={user.ResetPasswordToken}&email={user.Email}";

            // Send email
            await _emailService.SendPasswordResetEmail(user.Email, resetLink); // Use actual user email
        }
         
        public async Task ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _context.ApplicationUser
                .FirstOrDefaultAsync(u => u.Email == email && u.ResetPasswordToken == token);

            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
                throw new Exception("Invalid or expired token");
             
            var (hash, salt) = PasswordHelper.HashPassword(newPassword);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;   
            user.ResetPasswordToken = null;
            user.ResetTokenExpiry = null;

            await _context.SaveChangesAsync();
        }
    }
}