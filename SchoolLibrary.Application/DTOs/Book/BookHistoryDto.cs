namespace SchoolLibrary.Application.DTOs.Book
{
    public record BookHistoryDto
    (
        string BookTitle,
        int BookId,
        DateTime Date,
        string OperationType
    );
}
