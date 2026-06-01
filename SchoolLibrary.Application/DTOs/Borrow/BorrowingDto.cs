namespace SchoolLibrary.Application.DTOs.Borrow
{
    public record BorrowingDto
    (
        int Id,
        string LibraryItem,
        DateTime BorrowedDate,
        DateTime? DueDate,
        DateTime? ReturnDate
    );
}
