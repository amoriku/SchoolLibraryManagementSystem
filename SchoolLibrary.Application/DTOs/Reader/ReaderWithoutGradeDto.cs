namespace SchoolLibrary.Application.DTOs.Reader
{
    public record ReaderWithoutGradeDto
    (
        string Id,
        string FirstName,
        string LastName,
        string? MiddleName
    );
}
