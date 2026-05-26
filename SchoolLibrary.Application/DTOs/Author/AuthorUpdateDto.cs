using SchoolLibrary.Domain.ValueObjects;

namespace SchoolLibrary.Application.DTOs.Author
{
    public record AuthorUpdateDto
    (
        int Id, 
        string FirstName,
        string LastName,
        string? MiddleName,
        string? Pseudonym
    );
}
