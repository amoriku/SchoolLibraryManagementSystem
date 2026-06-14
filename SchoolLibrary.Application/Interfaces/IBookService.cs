using SchoolLibrary.Application.DTOs;
using SchoolLibrary.Application.DTOs.Book;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IBookService
    {
        Task<BookDto> CreateAsync(BookCreateDto dto, CancellationToken cancellationToken);
        Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Book?> GetByTitleAsync(string title, CancellationToken cancellationToken);
        Task<List<BookDto>> GetAllAsync(QueryDto query, CancellationToken cancellationToken);
        Task<List<BookHistoryDto>> GetBookHistoryAsync(int bookId, CancellationToken cancellationToken);
        Task<DateTime?> GetNearestAvailabilityDateAsync(int bookId, CancellationToken cancellationToken);
    }
}
