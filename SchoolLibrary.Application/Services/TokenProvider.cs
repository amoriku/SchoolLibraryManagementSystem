
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Infrastructure;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolLibrary.Application.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IConfiguration configuration;
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenProvider(
            IConfiguration configuration,
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.context = context;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(configuration["JwtConfig:Secret"]!));

            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);

            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                ..roles.Select(r => new Claim(ClaimTypes.Role, r))
            ];

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = configuration["JwtConfig:Issuer"],
                Audience = configuration["JwtConfig:Audience"],
                SigningCredentials = creds,
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JwtConfig:ExpiryInMinutes"))
            };

            var tokenHandler = new JsonWebTokenHandler();

            string token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public async Task SaveRefreshTokenAsync(ApplicationUser user, string refreshToken, CancellationToken cancellationToken)
        {
            var userRefreshToken = await context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == user.Id);

            if (userRefreshToken != null)
            {
                userRefreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(-1);
            }

            RefreshToken token = new()
            {
                Token = refreshToken,
                UserId = user.Id,
                User = user,
                ExpiresOnUtc = DateTime.UtcNow.AddDays(3),
            };

            context.RefreshTokens.Add(token);
            await context.SaveChangesAsync();
        }

        public async Task<bool> RevokeRefreshTokensAsync(CancellationToken cancellationToken)
        {
            var currentUserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
            {
                throw new ApplicationException("You can`t do this");
            }

            await context.RefreshTokens
                .Where(rt => rt.UserId == currentUserId)
                .ExecuteDeleteAsync();

            return true;
        }
    }
}
