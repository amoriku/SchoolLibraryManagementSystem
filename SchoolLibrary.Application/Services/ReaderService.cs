using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.DTOs.Reader;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Constants;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Domain.Shared;
using SchoolLibrary.Infrastructure;
using SchoolLibrary.Infrastructure.Migrations;

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

        public async Task<List<ReaderDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await userManager
                .GetUsersInRoleAsync(UserRoles.Reader);

            List<ReaderDto> readers = [];

            foreach (var user in users)
            {
                readers.Add(new ReaderDto(
                    user.Id,
                    user.FullName.FirstName,
                    user.FullName.LastName,
                    user.FullName.LastName,
                    string.Empty
                ));
            }

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
