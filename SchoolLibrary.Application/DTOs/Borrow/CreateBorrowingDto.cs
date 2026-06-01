namespace SchoolLibrary.Application.DTOs.Borrow
{
    public record CreateBorrowingDto
    (
        int ReservationId,
        DateTime? DueDate
    );
}
