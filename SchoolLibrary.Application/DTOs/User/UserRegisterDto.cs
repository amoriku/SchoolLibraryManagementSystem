using SchoolLibrary.Domain.ValueObjects;

namespace SchoolLibrary.Application.DTOs.User
{
    public record UserRegisterDto
    (
        string Username,
        string ? Email,
        string Password,
        FullName FullName
    );
}
