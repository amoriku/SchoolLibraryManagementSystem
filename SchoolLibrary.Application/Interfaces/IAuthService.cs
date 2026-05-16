using SchoolLibrary.Application.DTOs;
using SchoolLibrary.Application.DTOs.User;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(UserLoginDto dto, CancellationToken cancellationToken);
        Task<ApplicationUser?> RegisterAsync(UserRegisterDto dto, CancellationToken cancellationToken);
        string? GetCurrentUser();
    }
}
