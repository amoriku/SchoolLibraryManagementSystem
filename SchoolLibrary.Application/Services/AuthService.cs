using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.DTOs;
using SchoolLibrary.Application.DTOs.User;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
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

            return new TokenResponseDto
                (
                    AccessToken: accessToken,
                    RefreshToken: refreshToken
                );
            //IList<string> userRole = await userManager
            //    .GetRolesAsync(user);
            //string accessToken = string.Empty;
            //string refreshToken = jwtService.GenerateRefreshToken();

            //if (userRole.Count != 0)
            //{
            //    accessToken = jwtService.GenerateToken
            //        (user.UserName, user.Id, userRole.First());
            //}

            //if (user.RefreshToken != refreshToken
            //    || string.IsNullOrEmpty(user.RefreshToken))
            //{
            //    user.RefreshToken = refreshToken;
            //    await userManager.UpdateAsync(user);
            //}


        }

        public string? GetCurrentUser()
        {
            string? userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;

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
