using SchoolLibrary.Domain.ValueObjects;

namespace SchoolLibrary.Application.DTOs.Author
{
    public record AuthorUpdateDto(int Id, string? Pseudonym, FullName FullName);
}
