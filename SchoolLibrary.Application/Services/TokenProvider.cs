using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SchoolLibrary.Application.DTOs;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Shared;
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

        // Получение нового токена доступа для пользователя, если его время действия истекло
        public async Task<TokenResponseDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new UnauthorizedAccessException("Refresh token is missing");
            }

            var refreshTokenEntity = await context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

            if (refreshTokenEntity is null || !refreshTokenEntity.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            var user = refreshTokenEntity.User;
            refreshTokenEntity.IsRevoked = true;

            string newAccessToken = await CreateTokenAsync(user);
            string newRefreshToken = GenerateRefreshToken();

            await SaveRefreshTokenAsync(user, newRefreshToken, cancellationToken);

            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                bool isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

                httpContext.Response.Cookies.Append(
                    CookieHeaderNames.CookieHeaderNameAccessToken,
                    newAccessToken,
                    new CookieOptions
                    {
                        Secure = !isProduction,
                        SameSite = isProduction ? SameSiteMode.Lax : SameSiteMode.None,
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JwtConfig:ExpiryInMinutes"))
                    }
                );

                httpContext.Response.Cookies.Append(
                    CookieHeaderNames.CookieHeaderNameRefreshToken,
                    newRefreshToken,
                    new CookieOptions
                    {
                        Secure = !isProduction,
                        SameSite = isProduction ? SameSiteMode.Lax : SameSiteMode.None,
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddDays(30)
                    }
                );
            }

            return new TokenResponseDto(newAccessToken, newRefreshToken);
        }

        // Сохранение токена восстановления в базу данных
        public async Task SaveRefreshTokenAsync(ApplicationUser user, string refreshToken, CancellationToken cancellationToken)
        {
            // Получение активных токенов восстановления
            var activeTokens = await context.RefreshTokens
                .Where(rt => rt.UserId == user.Id && !rt.IsRevoked && rt.ExpiresOnUtc > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            foreach (var token in activeTokens)
            {
                token.IsRevoked = true;
            }

            RefreshToken newEntity = new()
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresOnUtc = DateTime.UtcNow.AddDays(30),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            context.RefreshTokens.Add(newEntity);

            await context.SaveChangesAsync(cancellationToken);
        }

        // Создание токена доступа для пользователя
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

        // Генерация токена восстановления
        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        // Аннулирование всех токенов восстановления
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
