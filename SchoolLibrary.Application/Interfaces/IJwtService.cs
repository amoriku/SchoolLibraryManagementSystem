//using Microsoft.AspNetCore.Identity;
//using SchoolLibrary.Application.DTOs;
//using SchoolLibrary.Domain.Entities;
//using SchoolLibrary.Infrastructure;

//namespace SchoolLibrary.Application.Interfaces
//{
//    public interface IJwtService
//    {
//        string GenerateRefreshToken();
//        string GenerateToken(string? username, string userId, string userRole);
//        Task<IDictionary<string, object>> ValidateTokenAsync(string token);
//        Task<ApplicationUser?> ValidateRefreshTokenAsync(AppDbContext context, string userId, string refreshToken);
//    }
//}
