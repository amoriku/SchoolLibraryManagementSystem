using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.DTOs.User;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Constants;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Domain.Shared;
using SchoolLibrary.Domain.ValueObjects;
using SchoolLibrary.Infrastructure;
using System.Security.Claims;

namespace SchoolLibrary.Application.Services
{
    public class UserService : BaseService<UserService>, IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(
            AppDbContext context, 
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserService> logger) : base(context, logger) 
        { 
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await userManager.Users
                .Where(u => !u.IsDeleted)
                .Select(u => new UserDto(
                    u.Id,
                    u.Email,
                    u.UserName,
                    context.UserRoles
                        .Where(ur => ur.UserId == u.Id)
                        .Join(context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                        .FirstOrDefault() ?? UserRoles.Guest
                ))
                .ToListAsync();

            return users;
        }

        public async Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(userId))
            {
                //logger.LogInformation($"{DebugMessages.ApplicationLayerMessage}: User not found");
                throw new NotFoundException("User not found");
            }

            return await context.ApplicationUsers
                .FirstOrDefaultAsync(au => au.Id == userId, cancellationToken);
        }

        public async Task DeleteAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new InvalidOperationException("Invalid user id");

            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.DeletedAt = DateTime.UtcNow;
                user.IsDeleted = true;

                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to delete user");
                }
            }
        }

        public async Task<ApplicationUser> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken)
        {
            string role = dto.Role.ToString();
            bool isDeleted = await userManager.Users
                .Where(u => u.IsDeleted || u.DeletedAt != null)
                .AnyAsync(cancellationToken);

            if (!isDeleted)
            {
                if (!string.IsNullOrEmpty(dto.Username)
                 && await userManager.FindByNameAsync(dto.Username) != null)
                {
                    throw new AlreadyExistsException("User already exists");
                }

                if (!string.IsNullOrEmpty(dto.Email)
                    && await userManager.FindByEmailAsync(dto.Email) != null)
                {
                    throw new AlreadyExistsException("User already exists");
                }
            }

            FullName fullName = new FullName(dto.FirstName, dto.LastName, dto.MiddleName);
            var user = new ApplicationUser
            {
                Email = string.IsNullOrEmpty(dto.Email) ? string.Empty : dto.Email,
                EmailConfirmed = true,
                UserName = dto.Username,
                FullName = fullName,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded && !string.IsNullOrEmpty(role))
            {
                await userManager.AddToRoleAsync(user, role);
            }

            return user;
        }

        //public string? GetCurrentUserId(CancellationToken cancellationToken)
        //{
        //    return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //}
    }
}
