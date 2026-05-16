using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken cancellationToken);
        Task<List<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken);
        //string? GetCurrentUserId(CancellationToken cancellationToken);
    }
}
