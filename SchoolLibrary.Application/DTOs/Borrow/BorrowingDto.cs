using SchoolLibrary.Application.DTOs.Reader;

namespace SchoolLibrary.Application.DTOs.Borrow
{
    public record BorrowingDto
    (
        int Id,
        string LibraryItem,
        int? LibraryItemId,
        ReaderWithoutGradeDto Reader,
        DateTime BorrowedDate,
        DateTime? DueDate,
        DateTime? ReturnDate
    );
}
