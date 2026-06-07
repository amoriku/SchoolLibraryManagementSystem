namespace SchoolLibrary.Application.DTOs.Borrow
{
    public record CreateDirectBorrowingDto
    (
        string ReaderId,
        int ItemId,
        DateTime? DueDate
    );
}
