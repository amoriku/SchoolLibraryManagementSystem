using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Domain.Shared;
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

        public async Task<List<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.ApplicationUsers.ToListAsync(cancellationToken);
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

        //public string? GetCurrentUserId(CancellationToken cancellationToken)
        //{
        //    return httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //}
    }
}
