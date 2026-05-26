namespace SchoolLibrary.Application.DTOs.User
{
    public record UserDeleteDto
    (
        string Id,
        bool IsDeleted,
        DateTime DeletedAt
    );
}
