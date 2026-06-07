using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.DTOs.Borrow;
using SchoolLibrary.Application.DTOs.Reader;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Infrastructure;

namespace SchoolLibrary.Application.Services
{
    public class BorrowingService : BaseService<BorrowingService>, IBorrowingService
    {
        private readonly IAuthService authService;
        private readonly IReaderService readerService;

        public BorrowingService(
            AppDbContext context, 
            ILogger<BorrowingService> logger, 
            IAuthService authService,
            IReaderService readerService
        )
            : base (context, logger)
        {
            this.authService = authService;
            this.readerService = readerService;
        }

        // Обработка возвращения книги в библиотеку (со стороны библиотекаря)
        public async Task<bool> ReturnAsync(int itemCopyId, CancellationToken cancellationToken)
        {
            // Проверяем статус текущей копии
            var itemCopy = await context.LibraryItemCopies
                .FirstOrDefaultAsync(ic => ic.Id == itemCopyId && ic.Status == Domain.ItemCopyStatus.Borrowed, cancellationToken);

            if (itemCopy == null)
            {
                throw new InvalidOperationException("Invalid item copy id or book is not borrowed");
            }

            // Проверка на то возвращена книга или нет.
            var activeBorrowing = await context.Borrowings
                .FirstOrDefaultAsync(b => b.LibraryItemCopyId == itemCopyId && b.ReturnedAt == null, cancellationToken);

            if (activeBorrowing == null)
            {
                throw new InvalidOperationException("Invalid borrowing record");
            }

            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                itemCopy.Status = Domain.ItemCopyStatus.Available;
                activeBorrowing.ReturnedAt = DateTime.UtcNow;
                var historyRecord = new CreateReaderHistoryDto(activeBorrowing.ReaderId, itemCopyId, Domain.OperationType.Return);

                await readerService.CreateReaderHistoryAsync(historyRecord, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<List<BorrowingDto>> GetActiveBorrowings(CancellationToken cancellationToken)
        {
            var user = await authService.GetCurrentUserAsync(cancellationToken);
            var borrowings = await context.Borrowings
                .Where(b => b.ReaderId == user.Id && b.ReturnedAt == null)
                .Select(b => new BorrowingDto
                (
                    b.Id,
                    b.LibraryItemCopy.LibraryItem.Title,
                    b.LibraryItemCopyId,
                    new ReaderWithoutGradeDto(
                        b.ReaderId,
                        b.Reader.FullName.FirstName,
                        b.Reader.FullName.LastName,
                        b.Reader.FullName.MiddleName
                    ),
                    b.BorrowedAt,
                    b.DueDate,
                    b.ReturnedAt
                ))
                .ToListAsync(cancellationToken);


            return borrowings;
        }

        public async Task<BorrowingDto> CreateAsync(CreateBorrowingDto dto, CancellationToken cancellationToken)
        {
            string targetReaderId;
            int targetBookId;
            Reservation? activeReservation = null;

            // Выдача по существующей онлайн-брони
            if (dto.ReservationId.HasValue)
            {                
                activeReservation = await context.Reservations
                    .Include(r => r.LibraryItem)
                    .FirstOrDefaultAsync(r => r.Id == dto.ReservationId.Value && r.IsActive, cancellationToken);

                if (activeReservation == null)
                {
                    throw new NotFoundException("Active reservation not found");
                }

                targetReaderId = activeReservation.ReaderId;
                targetBookId = activeReservation.LibraryItemId;
            }
            // Выдача человеку, который пришел без онлайн брони
            else if (!string.IsNullOrEmpty(dto.ReaderId) && dto.BookId.HasValue)
            {
                var readerExists = await context.ApplicationUsers
                    .AnyAsync(r => r.Id == dto.ReaderId, cancellationToken);

                if (!readerExists)
                {
                    throw new InvalidOperationException("Invalid reader");
                }

                // Проверка остатка, чтобы живая очередь не забрала чужую онлайн-бронь
                int availableCount = await context.LibraryItemCopies
                    .CountAsync(c => c.LibraryItemId == dto.BookId.Value && c.Status == Domain.ItemCopyStatus.Available, cancellationToken)
                    - await context.Reservations
                    .CountAsync(r => r.LibraryItemId == dto.BookId.Value && r.IsActive, cancellationToken);

                if (availableCount <= 0)
                {
                    throw new InvalidOperationException("Выдача невозможно: все свободные книги зарезервированы онлайн!");
                }

                targetReaderId = dto.ReaderId;
                targetBookId = dto.BookId.Value;
            }
            else
            {
                throw new InvalidOperationException("Invalid request parameters");
            }

            // Ищем первый доступный экземпляр
            var copyToBorrow = await context.LibraryItemCopies
                .FirstOrDefaultAsync(ic => ic.LibraryItemId == targetBookId && ic.Status == Domain.ItemCopyStatus.Available, cancellationToken);

            if (copyToBorrow == null)
            {
                throw new InvalidOperationException("No physical copies available at the moment");
            }

            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                // Если выдача была по брони, то переводим бронь в неактивную
                if (activeReservation != null)
                {
                    activeReservation.IsActive = false;
                }

                // Меняем статус физической копии книги
                copyToBorrow.Status = Domain.ItemCopyStatus.Borrowed;

                // Создаем запись операционного учета в Borrowings
                Borrowing newBorrowing = new Borrowing
                {
                    BorrowedAt = DateTime.UtcNow,
                    DueDate = dto.DueDate ?? DateTime.UtcNow.AddDays(14), 
                    ReaderId = targetReaderId,
                    LibraryItemCopyId = copyToBorrow.Id,
                };
                context.Borrowings.Add(newBorrowing);

                // Лог записи в таблицу истории читателей
                var historyRecord = new CreateReaderHistoryDto(targetReaderId, copyToBorrow.Id, Domain.OperationType.Borrowing);

                await readerService.CreateReaderHistoryAsync(historyRecord, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                // Подгружаем данные пользователя для формирования красивого DTO ответа
                var readerInfo = await context.ApplicationUsers
                    .FirstAsync(u => u.Id == targetReaderId, cancellationToken);

                // Название книги берем либо из брони, либо прямым запросом из каталога
                string bookTitle = activeReservation?.LibraryItem.Title
                    ?? await context.Books.Where(b => b.Id == targetBookId).Select(b => b.Title).FirstAsync(cancellationToken);

                return new BorrowingDto(
                    newBorrowing.Id,
                    bookTitle,
                    newBorrowing.LibraryItemCopyId,
                    new ReaderWithoutGradeDto(
                        readerInfo.Id,
                        readerInfo.FullName.FirstName,
                        readerInfo.FullName.LastName,
                        readerInfo.FullName.MiddleName
                    ),
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
                .Where(b => b.ReturnedAt == null)
                .Select(b => new BorrowingDto(
                    b.Id,
                    b.LibraryItemCopy.LibraryItem.Title,
                    b.LibraryItemCopyId,
                    new ReaderWithoutGradeDto(
                        b.ReaderId,
                        b.Reader.FullName.FirstName,
                        b.Reader.FullName.LastName,
                        b.Reader.FullName.MiddleName
                    ),
                    b.BorrowedAt,
                    b.DueDate,
                    b.ReturnedAt
                ))
                .ToListAsync(cancellationToken);

            return borrowings;
        }
    }
}
