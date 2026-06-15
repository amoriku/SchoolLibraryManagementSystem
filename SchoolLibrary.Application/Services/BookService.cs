using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.Common;
using SchoolLibrary.Application.DTOs;
using SchoolLibrary.Application.DTOs.Author;
using SchoolLibrary.Application.DTOs.Book;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Infrastructure;
using SchoolLibrary.Infrastructure.Migrations;

namespace SchoolLibrary.Application.Services
{
    public class BookService : BaseService<BookService>, IBookService
    {
        private readonly IAuthService authService;

        public BookService(IAuthService authService,AppDbContext context, ILogger<BookService> logger) : base(context, logger) 
        {
            this.authService = authService;
        }


        public async Task<DateTime?> GetNearestAvailabilityDateAsync(int bookId, CancellationToken cancellationToken)
        {
            var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
            string currentUserId = currentUser.Id;

            // 1. Проверяем, существуют ли физические копии
            var copyIds = await context.LibraryItemCopies
                .Where(ic => ic.LibraryItemId == bookId)
                .Select(ic => ic.Id)
                .ToListAsync(cancellationToken);

            int totalCopiesCount = copyIds.Count;
            if (totalCopiesCount == 0) return null;

            // 2. Считаем, сколько людей заняли эту книгу РАНЬШЕ текущего пользователя
            var userReservation = await context.Reservations
                .FirstOrDefaultAsync(r => r.IsActive && r.LibraryItemId == bookId && r.ReaderId == currentUserId, cancellationToken);

            int peopleAhead = 0;
            if (userReservation != null)
            {
                peopleAhead = await context.Reservations
                    .CountAsync(r => r.IsActive && r.LibraryItemId == bookId && r.ReservedAt < userReservation.ReservedAt, cancellationToken);
            }
            else
            {
                peopleAhead = await context.Reservations
                    .CountAsync(r => r.IsActive && r.LibraryItemId == bookId, cancellationToken);
            }

            // 3. Считаем свободные копии на текущий момент времени
            int availableCopiesCount = await context.LibraryItemCopies
                .CountAsync(ic => ic.LibraryItemId == bookId && ic.Status == Domain.ItemCopyStatus.Available, cancellationToken);

            // Если книги на полке есть, и их больше, чем людей перед нами (или мы первые),
            // значит экземпляр свободен для нас прямо сейчас
            if (availableCopiesCount > peopleAhead)
            {
                return DateTime.UtcNow; // Книга доступна прямо сейчас
            }

            // 4. Логика для больших очередей
            if (peopleAhead > totalCopiesCount * 2)
            {
                const int averageReadingDays = 14;
                double cycles = (double)peopleAhead / totalCopiesCount;
                int estimatedDaysWaiting = (int)Math.Ceiling(cycles * averageReadingDays);

                return DateTime.UtcNow.AddDays(estimatedDaysWaiting);
            }

            // 5. Стандартная логика для маленьких очередей (когда книги на полке кончились)
            var nearestBorrowingDueDate = await context.Borrowings
                .Where(b => copyIds.Contains(b.LibraryItemCopyId) && b.ReturnedAt == null)
                .OrderBy(b => b.DueDate)
                .Select(b => b.DueDate)
                .FirstOrDefaultAsync(cancellationToken);

            var nearestReservationExpiration = await context.Reservations
                .Where(r => r.IsActive && r.LibraryItemId == bookId)
                .OrderBy(r => r.ReservedAt)
                .Select(r => (DateTime?)r.ReservedAt.AddDays(2))
                .FirstOrDefaultAsync(cancellationToken);

            if (nearestBorrowingDueDate != null && nearestReservationExpiration != null)
            {
                return nearestBorrowingDueDate < nearestReservationExpiration ? nearestBorrowingDueDate : nearestReservationExpiration;
            }

            return nearestBorrowingDueDate ?? nearestReservationExpiration ?? DateTime.UtcNow.AddDays(14);
        }



        public async Task<List<BookHistoryDto>> GetBookHistoryAsync(int bookId, CancellationToken cancellationToken)
        {
            var book = await context.Books
                .FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);

            if (book == null)
            {
                logger.LogError("Book with id {bookId} doesnt exist", bookId);
                throw new InvalidOperationException("Invalid book");
            }

            var bookHistory = await context.UserHistories
                .Where(uh => uh.LibraryItemCopy.LibraryItemId == bookId)
                .Select(uh => new BookHistoryDto(book.Title, bookId, uh.Date, uh.OperationType.ToString()))
                .ToListAsync(cancellationToken);

            return bookHistory;
        }

        public async Task<BookDto> CreateAsync(BookCreateDto dto, CancellationToken cancellationToken)
        {
            Book? bookInstance = await context.Books
                .FirstOrDefaultAsync(b => b.Title == dto.Title, cancellationToken);

            // Создаем книгу если её нет в базе
            if (bookInstance == null)
            {
                // Проверка на существование авторов
                var existingAuthorIds = await context.Authors
                    .Where(a => dto.AuthorIds.Contains(a.Id))
                    .Select(a => a.Id)
                    .ToListAsync(cancellationToken);

                // Если каких-то авторов не существует выводим сообщение об ошибке
                if (existingAuthorIds.Count != dto.AuthorIds.Count)
                {
                    string message = "Some of authors dont exist";

                    logger.LogWarning(message);
                    throw new NotFoundException(message);
                }

                // Создание книги
                bookInstance = new Book
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    PublishedYear = dto.PublishedYear,
                    Price = dto.Price,
                    ItemAuthors = dto.AuthorIds.Select(id => new ItemAuthor
                    {
                        AuthorId = id,
                    }).ToList(),
                    ISBN_13 = !string.IsNullOrEmpty(dto.Isbn) ? dto.Isbn : DataGenerator.GenerateIsbn13(),
                };

                context.Books.Add(bookInstance);
                await context.SaveChangesAsync();
            }
            
            for (int i = 0; i < dto.Quantity; i++)
            {
                var itemCopy = new LibraryItemCopy
                {
                    FundId = 1,
                    LibraryItem = bookInstance,
                    Status = Domain.ItemCopyStatus.Available,
                    InventoryCode = $"SLIC-{bookInstance.Id}-{DateTime.UtcNow.Ticks.ToString().Substring(11)}"
                };

                context.LibraryItemCopies.Add(itemCopy);
            }

            await context.SaveChangesAsync(cancellationToken);

            var authors = bookInstance.ItemAuthors
                .Where(ia => ia.LibraryItemId == bookInstance.Id)
                .Join(context.Authors, ia => ia.Id, a => a.Id, (ia, a) => new AuthorDto(
                    ia.AuthorId,
                    ia.Author.FullName.FirstName,
                    ia.Author.FullName.LastName,
                    ia.Author.FullName.MiddleName
                ))
                .ToList();

            return new BookDto(
                bookInstance.Id,
                bookInstance.ReceiptDate,
                bookInstance.Title,
                bookInstance.PublishedYear,
                bookInstance.LibraryItemCopies.Count(),
                authors
            );
        }

        public async Task<List<BookDto>> GetAllAsync(QueryDto query, CancellationToken cancellationToken)
        {
            var booksQuery = context.Books.AsNoTracking();

            if (!string.IsNullOrEmpty(query.Search))
            {
                booksQuery = booksQuery.Where(b => b.Title.Contains(query.Search));
            }
            var books = await booksQuery
                .Select(b => new BookDto
                (
                    b.Id,
                    b.ReceiptDate,
                    b.Title,
                    //b.Description,
                    b.PublishedYear,

                    b.LibraryItemCopies.Count(c => c.Status == Domain.ItemCopyStatus.Available) 
                    - b.Reservations.Count(r => r.IsActive),

                    b.ItemAuthors
                        .Select(ia => new AuthorDto(
                            ia.AuthorId,
                            ia.Author.FullName.FirstName,
                            ia.Author.FullName.LastName,
                            ia.Author.FullName.MiddleName
                        ))
                        .ToList()
                //b.Price
                ))
                .ToListAsync(cancellationToken);


            return books;
        }

        public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Books
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByTitleAsync(string title, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("Invalid title");
            }

            return await context.Books
                .Where(b => b.Title == title)
                .FirstOrDefaultAsync();
        }

        // Gets all time borrowings from UserHistories
        public async Task<List<BookHistoryDto>> GetBorrowingHistoriesAsync(CancellationToken cancellationToken)
        {
            var borrowingHistories = await context.UserHistories
                .AsNoTracking()
                .Where(uh => uh.OperationType == Domain.OperationType.Borrowing)
                .Select(uh => new BookHistoryDto(
                    uh.LibraryItemCopy.LibraryItem.Title,
                    uh.LibraryItemCopy.LibraryItemId,
                    uh.Date,
                    uh.OperationType.ToString()
                    ))
                .ToListAsync(cancellationToken);

            return borrowingHistories;
        }

        // Gets all time returns from UserHistories
        public async Task<List<BookHistoryDto>> GetReturnHistoriesAsync(CancellationToken cancellationToken)
        {
            var returnHistories = await context.UserHistories
                .AsNoTracking()
                .Where(uh => uh.OperationType == Domain.OperationType.Return)
                .Select(uh => new BookHistoryDto(
                    uh.LibraryItemCopy.LibraryItem.Title,
                    uh.LibraryItemCopy.LibraryItemId,
                    uh.Date,
                    uh.OperationType.ToString()
                    ))
                .ToListAsync(cancellationToken);

            return returnHistories;
        }
    }
}
