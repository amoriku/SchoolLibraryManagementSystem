using SchoolLibrary.Application.DTOs.Author;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IAuthorService 
    {
        Task<Author> CreateAsync(AuthorCreateDto dto, CancellationToken cancellationToken);
        Task<Author> UpdateAsync(AuthorUpdateDto dto, CancellationToken cancellationToken);
        Task<Author?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<AuthorDto>> GetAllAsync(CancellationToken cancellationToken);
    }
}
