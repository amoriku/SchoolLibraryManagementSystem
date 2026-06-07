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

        // Операции для читателя сейчас:
        // - Loan || Borrow (Выдача, заимствование)
        // - Return (Возврат)
        // - History of reader (История пользователя) a.k.a электронный формуляр
        public async Task<ReaderHistoryDto> CreateReaderHistoryAsync(CreateReaderHistoryDto dto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(dto.ReaderId))
            {
                throw new InvalidOperationException("Invalid reader");
            }

            var item = await context.LibraryItemCopies
                .Include(ic => ic.LibraryItem)
                .Select(ic => ic.LibraryItem)
                .FirstOrDefaultAsync(cancellationToken);

            if (item is null)
            {
                throw new InvalidOperationException("Invalid library item");
            }

            UserHistory newHistoryRecord = new()
            {
                Date = DateTime.UtcNow,
                LibraryItemCopyId = dto.LibraryItemCopyId,
                OperationType = dto.OperationType,
                UserId = dto.ReaderId,
            };

            context.UserHistories.Add(newHistoryRecord);
            await context.SaveChangesAsync();


            return new ReaderHistoryDto(
                newHistoryRecord.UserId,
                item.Title,
                newHistoryRecord.Date,
                newHistoryRecord.OperationType.ToString()
            );
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
            FullName fullName = new FullName(dto.FirstName, dto.LastName, dto.MiddleName);
            string password = $"{username[0]}_1234";

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
                        string.IsNullOrEmpty(u.UserName) ? "-" : u.UserName
                    )
                )
                .ToListAsync(cancellationToken);

            return readers;
        }
        public async Task<List<ReaderHistoryDto>> GetReaderHistoryAsync(string readerId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(readerId))
            {
                throw new NotFoundException($"Invalid reader");
            }

            var readerHistoryRecords = await context.UserHistories
                .AsNoTracking()
                .Where(uh => uh.UserId == readerId)
                .Select(uh => new ReaderHistoryDto(
                    readerId,
                    uh.LibraryItemCopy.LibraryItem.Title,
                    uh.Date,
                    uh.OperationType.ToString()
                ))
                .ToListAsync(cancellationToken);

            return readerHistoryRecords;
        }
    }
}
