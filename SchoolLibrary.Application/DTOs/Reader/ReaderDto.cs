namespace SchoolLibrary.Application.DTOs.Reader
{
    public record ReaderDto
    (
        string Id,
        string FirstName,
        string LastName,
        string? MiddleName,
        string GradeName,
        string Username
    );
}
