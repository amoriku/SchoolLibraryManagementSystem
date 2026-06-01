using SchoolLibrary.Application.DTOs;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Application.Interfaces
{
    public interface ITokenProvider 
    {
        Task<string> CreateTokenAsync(ApplicationUser user);
        string GenerateRefreshToken();
        Task SaveRefreshTokenAsync(ApplicationUser user, string refreshToken, CancellationToken cancellationToken);
        Task<TokenResponseDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
        Task<bool> RevokeRefreshTokensAsync(CancellationToken cancellationToken);
    }
}
