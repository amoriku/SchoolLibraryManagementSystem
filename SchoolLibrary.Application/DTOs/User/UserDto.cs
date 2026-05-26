namespace SchoolLibrary.Application.DTOs.User
{
    public record UserDto
    (
        string Id,
        string? Email,
        string? Username,
        string? Role
    );
}
