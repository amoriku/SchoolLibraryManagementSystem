namespace SchoolLibrary.Application.DTOs.Reserve
{
    public record ReserveWithoutReaderDto
    (
        int ReserveId,
        string LibraryItem,
        DateTime ReservationDate
    );
}
