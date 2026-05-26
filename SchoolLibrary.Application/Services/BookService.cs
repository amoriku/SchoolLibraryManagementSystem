using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            bool bookExists = await context.Books
                .AnyAsync(
                    b => b.Title == dto.Title
                    && b.ItemAuthors.Any(ia => dto.AuthorIds.Contains(ia.AuthorId)),
                    cancellationToken
                );

            // Если у автора уже есть книга, то вывод ошибки о том, что она уже есть в базе
            if (bookExists)
            {
                logger.LogWarning("Book {BookName} already exists", dto.Title);
                throw new Exception("Book already exists");
            }

            // Проверка на существование авторов
            var existingAuthorIds = await context.Authors
                .Where(a => dto.AuthorIds.Contains(a.Id))
                .Select(a => a.Id)
                .ToListAsync(cancellationToken);

            // Если каких-то авторов не существует вывод об ошибке
            if (existingAuthorIds.Count != dto.AuthorIds.Count)
            {
                string message = "Some of authors dont exist";

                logger.LogWarning(message);
                throw new NotFoundException(message);
            }


            // Создание книги
            Book book = new Book
            {
                Title = dto.Title,
                Description = dto.Description,
                PublishedYear = dto.PublishedYear,
                Price = dto.Price,
                ItemAuthors = dto.AuthorIds.Select(id => new ItemAuthor
                {
                    AuthorId = id,
                }).ToList()
            };

            context.Books.Add(book);
            await context.SaveChangesAsync(cancellationToken);

            var authors = book.ItemAuthors
                .Where(ia => ia.LibraryItemId == book.Id)
                .Join(context.Authors, ia => ia.Id, a => a.Id, (ia, a) => new AuthorDto(
                    ia.AuthorId,
                    ia.Author.FullName.FirstName,
                    ia.Author.FullName.LastName,
                    ia.Author.FullName.MiddleName
                ))
                .ToList();

            return new BookDto(
                book.Id, 
                book.ReceiptDate, 
                book.Title, 
                book.PublishedYear, 
                authors
            );
        }

        public async Task<List<BookDto>> GetAllAsync(QueryDto query, CancellationToken cancellationToken)
        {
            var books = await context.Books
                .Where(b => b.Title.Contains(
                    string.IsNullOrEmpty(query.Search)
                    ? string.Empty
                    : query.Search)
                )
                .Select(b => new BookDto
                (
                    b.Id,
                    b.ReceiptDate,
                    b.Title,
                    //b.Description,
                    b.PublishedYear,
                    context.ItemAuthors
                        .Where(ia => ia.LibraryItemId == b.Id)
                        .Select(ia => new AuthorDto(
                            ia.AuthorId,
                            ia.Author.FullName.FirstName,
                            ia.Author.FullName.LastName,
                            ia.Author.FullName.MiddleName
                            ))
                        .ToList()
                //b.Price
                ))
                .ToListAsync();


            return books;

            //var query = context.Books.AsQueryable();
            
            //if (!string.IsNullOrEmpty(title))
            //{
            //    query = query
            //        .Where(b => b.Title.Contains(title));

                
            //}

            //return await query.ToListAsync(cancellationToken);
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
