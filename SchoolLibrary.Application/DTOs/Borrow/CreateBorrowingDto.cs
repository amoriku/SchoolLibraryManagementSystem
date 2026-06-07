namespace SchoolLibrary.Application.DTOs.Borrow
{
    public record CreateBorrowingDto
    (
        string? ReaderId,
        int? BookId,
        int? ReservationId,
        DateTime? DueDate
    );
}
