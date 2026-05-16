using SchoolLibrary.Domain.ValueObjects;

namespace SchoolLibrary.Application.DTOs.Author
{
    public record AuthorCreateDto
    (
        string FirstName,
        string LastName,
        string? MiddleName,
        string? Pseudonym
    );
}
