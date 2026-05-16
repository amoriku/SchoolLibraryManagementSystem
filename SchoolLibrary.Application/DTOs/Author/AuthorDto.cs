using SchoolLibrary.Domain.ValueObjects;

namespace SchoolLibrary.Application.DTOs.Author
{
    public record AuthorDto
    (
        int id,
        string FirstName,
        string LastName,
        string? MiddleName
    );
}
