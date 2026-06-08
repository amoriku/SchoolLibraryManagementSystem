using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.DTOs;
using SchoolLibrary.Application.DTOs.User;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Application.Shared;
using SchoolLibrary.Domain.Constants;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Infrastructure;
using System.Security.Claims;

namespace SchoolLibrary.Application.Services
{
    /// <summary>
    /// Сервис для обработки логики входа и регистрации в системе.
    /// </summary>
    public class AuthService : BaseService<AuthService>, IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenProvider tokenProvider;
        private readonly IHttpContextAccessor httpContextAccessor;

        //private readonly IJwtService jwtService;

        // Первичная подгрузка сервисов
        public AuthService(
            AppDbContext context,
            ILogger<AuthService> logger,
            //IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor,
            ITokenProvider tokenProvider,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
            : base(context, logger)
        {
            this.tokenProvider = tokenProvider;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.httpContextAccessor = httpContextAccessor;
            //this.jwtService = jwtService;
        }

        // Вход в приложение и выдача токенов доступа и обновления.
        public async Task<TokenResponseDto?> LoginAsync(UserLoginDto dto, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(dto.Identifier);

            if (user is null)
            {
                user = await userManager.FindByNameAsync(dto.Identifier);
            }

            if (user is null || !await userManager.CheckPasswordAsync(user, dto.Password))
            {
                throw new UnauthorizedAccessException("Invalid user or password");
            }

            string accessToken = await tokenProvider.CreateTokenAsync(user);
            string refreshToken = tokenProvider.GenerateRefreshToken();

            await tokenProvider.SaveRefreshTokenAsync(user, refreshToken, cancellationToken);

            bool isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

            // Добавление сессии пользователя в куки браузера для верификации пользователя.
            httpContextAccessor.HttpContext.Response.Cookies.Append(CookieHeaderNames.CookieHeaderNameAccessToken, accessToken,
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddMinutes(8),
                    Secure = !isProduction,
                    SameSite = isProduction ? SameSiteMode.Lax : SameSiteMode.None,
                    HttpOnly = true,
                }
            );

            httpContextAccessor.HttpContext.Response.Cookies.Append(CookieHeaderNames.CookieHeaderNameRefreshToken, refreshToken,
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(30),
                    Secure = !isProduction,
                    SameSite = isProduction ? SameSiteMode.Lax : SameSiteMode.None,
                    HttpOnly = true,
                }
            );

            return new TokenResponseDto
                (
                    AccessToken: accessToken,
                    RefreshToken: refreshToken
                );
        }

        public async Task<TokenResponseDto> RefreshAsync(CancellationToken cancellationToken)
        {
            httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(CookieHeaderNames.CookieHeaderNameRefreshToken, out string refreshToken);

            var tokenResponse = await tokenProvider.RefreshAsync(refreshToken, cancellationToken);
            return tokenResponse;
        }

        // Обработка выхода из приложения.
        public async Task Logout(CancellationToken cancellationToken)
        {
            // Получение и очистка сессии пользователя в случае, если он авторизован.
            string? userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await tokenProvider.RevokeRefreshTokensAsync(cancellationToken);

            if (!string.IsNullOrEmpty(userId))
            {
                httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieHeaderNames.CookieHeaderNameAccessToken, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(-1),
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    HttpOnly = true,
                });

                httpContextAccessor.HttpContext.Response.Cookies.Delete(CookieHeaderNames.CookieHeaderNameRefreshToken, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(-1),
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    HttpOnly = true,
                });
            }
        }

        // Получение текущего пользователя
        public async Task<UserDto> GetCurrentUserAsync(CancellationToken cancellationToken)
        {
            string? userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    return new UserDto(user.Id, user.Email, user.UserName!, roles[0]);
                }
            }

            throw new NotFoundException("User not found");
        }

        public async Task<ApplicationUser?> RegisterAsync(UserRegisterDto dto, CancellationToken cancellationToken)
        {
            string defaultRoleName = UserRoles.Reader;

            var user = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                EmailConfirmed = true,
                FullName = new(
                    firstName: dto.FullName.FirstName,
                    lastName: dto.FullName.LastName,
                    middleName: dto.FullName.MiddleName ?? string.Empty
                ),
                CreatedAt = DateTimeOffset.UtcNow,
                LastModifiedAt = DateTimeOffset.UtcNow,
            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                string errorCode = result.Errors.First().Code;

                if (errorCode == "DuplicateUserName" || errorCode == "DuplicateEmail")
                {
                    throw new AlreadyExistsException("Username or email already taken");
                }

                throw new Exception(result.Errors.First().Description);
            }
            await userManager.AddToRoleAsync(user, defaultRoleName);
            return user;
        }
    }
}
