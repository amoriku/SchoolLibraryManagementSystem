using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NickBuhro.Translit;
using SchoolLibrary.Application.DTOs.Reader;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Constants;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Domain.Shared;
using SchoolLibrary.Domain.ValueObjects;
using SchoolLibrary.Infrastructure;
using SchoolLibrary.Infrastructure.Migrations;
using System.Security.Cryptography.X509Certificates;

namespace SchoolLibrary.Application.Services
{
    public class ReaderService : BaseService<ReaderService>, IReaderService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ReaderService(
            AppDbContext context, 
            ILogger<ReaderService> logger,
            UserManager<ApplicationUser> userManager
        ) 
            : base(context, logger) 
        {
            this.userManager = userManager;
        }

        // So operations for reader are:
        // - Loan || Borrow (Выдача, заимствование)
        // - Return (Возврат)
        // - History of reader (История пользователя) a.k.a электронный формуляр
        public async Task<UserHistory> CreateUserHistoryRecordAsync(UserHistoryCreateDto dto, CancellationToken cancellationToken)
        {
            if (dto.UserId is null)
            {
                throw new NotFoundException($"{DebugMessages.ApplicationLayerMessage}.ReaderService Invalid user or library item");
            }

            UserHistory record = new UserHistory
            {
                UserId = dto.UserId,
                Date = DateTime.UtcNow,
                LibraryItemCopyId = dto.LibraryItemCopyId,
            };


            return null;
        }

        public async Task<ReaderDto> CreateAsync(CreateReaderDto dto, CancellationToken cancellationToken)
        {
            string? gradeName = await context.Grades
                .Where(g => g.Id == dto.GradeId)
                .Select(g => g.DisplayName)
                .FirstOrDefaultAsync(cancellationToken);

            if (string.IsNullOrEmpty(gradeName))
            {
                throw new InvalidOperationException($"Grade with {dto.GradeId} not exists");
            }

            string specialSymbols = "~!@#$%^&*()_+={}|:;'<,>.?/";
            string randomSymbol = specialSymbols[Random.Shared.Next(0, specialSymbols.Length)].ToString();

            string russianRaw = $"{dto.LastName}_{gradeName}".Replace("-", "");
            string translited = Transliteration.CyrillicToLatin(russianRaw, Language.Russian);
 
            string username = $"{translited}_{Random.Shared.Next(1, 99999)}";
            string password = $"{username}{randomSymbol}";
            FullName fullName = new FullName(dto.FirstName, dto.LastName, dto.MiddleName);

            var user = new ApplicationUser
            {
                UserName = username,
                EmailConfirmed = true,
                GradeId = dto.GradeId,
                FullName = fullName,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, UserRoles.Reader);
            }

            return new ReaderDto(
                user.Id,
                user.FullName.FirstName,
                user.FullName.LastName,
                user.FullName.MiddleName,
                gradeName,
                user.UserName
            );
        }

        public async Task<List<ReaderDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var readers = await context.Users
                .Include(u => u.Grade)
                .Where(u => context.UserRoles
                    .Any(ur => ur.UserId == u.Id
                        && context.Roles.Any(r => r.Id == ur.RoleId && r.Name == UserRoles.Reader))
                )
                .Select(u => new ReaderDto(
                        u.Id,
                        u.FullName.FirstName,
                        u.FullName.LastName,
                        u.FullName.MiddleName,
                        u.Grade != null ? u.Grade.DisplayName : "-",
                        u.UserName
                    )
                )
                .ToListAsync(cancellationToken);

            return readers;
        }
        public async Task<UserHistory?> GetUserHistoryAsync(string userId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new NotFoundException($"{DebugMessages.ApplicationLayerMessage}.ReaderService Invalid user");
            }

            UserHistory? userHistory = await context.UserHistories
                .FirstOrDefaultAsync(uh => uh.UserId == userId, cancellationToken);

            return userHistory;
        }
    }
}
