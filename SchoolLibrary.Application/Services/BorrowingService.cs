using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.DTOs.Borrow;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Infrastructure;

namespace SchoolLibrary.Application.Services
{
    public class BorrowingService : BaseService<BorrowingService>, IBorrowingService
    {
        public BorrowingService(AppDbContext context, ILogger<BorrowingService> logger)
            : base (context, logger)
        {

        }

        public async Task<BorrowingDto> CreateAsync(CreateBorrowingDto dto, CancellationToken cancellationToken)
        {
            // Поиск активных запросов на бронь
            var reservation = await context.Reservations
                .FirstOrDefaultAsync(r => r.Id == dto.ReservationId && r.IsActive, cancellationToken);

            if (reservation == null)
            {
                throw new NotFoundException("Active reservation not found");
            }


            // Взятие первого доступного экземпляра для выдачи
            var copyToBorrow = await context.LibraryItemCopies
                .FirstOrDefaultAsync(ic => 
                    ic.LibraryItemId == reservation.LibraryItemId
                    && ic.Status == Domain.ItemCopyStatus.Available, 
                    cancellationToken
                );

            // Если нет - то сообщение об отсутствии
            if (copyToBorrow == null)
            {
                throw new InvalidOperationException("No physical copies available at the moment");
            }

            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                // Создание записи в истории читателя и выдачи
                reservation.IsActive = false;
                copyToBorrow.Status = Domain.ItemCopyStatus.Borrowed;

                Borrowing newBorrowing = new Borrowing
                {
                    BorrowedAt = DateTime.UtcNow,
                    DueDate = dto.DueDate != null ? dto.DueDate : DateTime.UtcNow.AddDays(14),
                    ReaderId = reservation.ReaderId,
                    LibraryItemCopyId = copyToBorrow.Id,
                };

                UserHistory newHistory = new UserHistory
                {
                    Date = DateTime.UtcNow,
                    LibraryItemCopyId = copyToBorrow.Id,
                    OperationType = Domain.OperationType.Borrowing,
                    UserId = reservation.ReaderId,
                };

                context.Borrowings.Add(newBorrowing);
                context.UserHistories.Add(newHistory);

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new BorrowingDto(
                    newBorrowing.Id, 
                    reservation.LibraryItem.Title, 
                    newBorrowing.BorrowedAt, 
                    newBorrowing.DueDate,
                    null
                );
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        // Запрос на получение всех выдач (за все время)
        public async Task<List<BorrowingDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var query = context.Borrowings.AsNoTracking();

            var borrowings = await query
                .Select(b => new BorrowingDto(
                    b.Id,
                    b.LibraryItemCopy.LibraryItem.Title,
                    b.BorrowedAt,
                    b.DueDate,
                    b.ReturnedAt
                ))
                .ToListAsync(cancellationToken);

            return borrowings;
        }
    }
}
