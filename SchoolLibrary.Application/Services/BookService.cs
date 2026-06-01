using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.Common;
using SchoolLibrary.Application.DTOs;
using SchoolLibrary.Application.DTOs.Author;
using SchoolLibrary.Application.DTOs.Book;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Infrastructure;

namespace SchoolLibrary.Application.Services
{
    public class BookService : BaseService<BookService>, IBookService
    {
        public BookService(AppDbContext context, ILogger<BookService> logger) : base(context, logger) { }

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
    }
}
