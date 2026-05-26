using SchoolLibrary.Application.DTOs.User;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken cancellationToken);
        Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<ApplicationUser> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken);
        Task DeleteAsync(string userId);
        //string? GetCurrentUserId(CancellationToken cancellationToken);
    }
}
