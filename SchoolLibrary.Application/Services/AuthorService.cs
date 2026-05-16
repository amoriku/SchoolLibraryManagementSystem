using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Application.DTOs.Author;
using SchoolLibrary.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Domain.Shared;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Domain.ValueObjects;

namespace SchoolLibrary.Application.Services
{
    /// <summary>
    /// Сервис обработки логики сущности "Автор"
    /// </summary>
    public class AuthorService : BaseService<AuthorService>, IAuthorService
    {
        public AuthorService(AppDbContext context, ILogger<AuthorService> logger) : base(context, logger) { }

        /// <summary>
        /// Создает автора
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Author> CreateAsync(AuthorCreateDto dto, CancellationToken cancellationToken)
        {
            bool exists = await context.Authors
                .AnyAsync(
                    a => a.Pseudonym == dto.Pseudonym
                    || 
                    a.FullName.FirstName == dto.FirstName
                    && a.FullName.LastName == dto.LastName,
                    cancellationToken
                );
            var fullName = new FullName(dto.FirstName, dto.LastName, dto.MiddleName);

            if (exists)
            {
                logger.LogInformation("Author {FullName} already exists", fullName);
                throw new AlreadyExistsException($"Author already exists");
            }

            Author author = new Author
            {
                Pseudonym = dto.Pseudonym,
                FullName = fullName
            };

            context.Authors.Add(author);
            await context.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Author {FullName} successfully added", fullName);

            return author;
        }

        /// <summary>
        /// Возвращает автора по его идентификатору 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var author = await context.Authors
                .FirstOrDefaultAsync(a => a.Id == Convert.ToInt32(id), cancellationToken);

            if (author is null)
            {
                logger.LogWarning("Author with id {Id} not found", id);
                throw new NotFoundException($"Author not found");
            }

            return author;
        }

        /// <summary>
        /// Получает всех авторов
        /// </summary>
        /// <returns></returns>
        public async Task<List<AuthorDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var authors = await context.Authors
                .Select(a => new AuthorDto(a.Id, a.FullName.FirstName, a.FullName.LastName, a.FullName.MiddleName))
                .ToListAsync();

            return authors;
        }

        public async Task<Author> UpdateAsync(AuthorUpdateDto dto, CancellationToken cancellationToken)
        {
            var author = await context.Authors
                .FirstOrDefaultAsync(a => a.Id == dto.Id, cancellationToken);

            if (author is null)
            {
                throw new NotFoundException($"Author not found");
            }
            else
            {
                author.Pseudonym = dto.Pseudonym;
                author.FullName = dto.FullName;
            }

            await context.SaveChangesAsync(cancellationToken);

            return author;
        }
    }
}
