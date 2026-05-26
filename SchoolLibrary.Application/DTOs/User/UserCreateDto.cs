using SchoolLibrary.Domain;
using SchoolLibrary.Domain.Constants;

namespace SchoolLibrary.Application.DTOs.User
{
    public record UserCreateDto
    (
        string LastName,
        string FirstName,
        string Password,
        string Role,
        string? MiddleName,
        string? Username,
        string? Email
    );
}
