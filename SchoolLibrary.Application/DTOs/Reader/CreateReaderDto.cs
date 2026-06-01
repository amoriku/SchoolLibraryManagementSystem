namespace SchoolLibrary.Application.DTOs.Reader
{
    public record CreateReaderDto
    (
        string FirstName,
        string LastName,
        string? MiddleName,
        short GradeId
    );
}
