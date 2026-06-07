using SchoolLibrary.Application.DTOs.Book;
using SchoolLibrary.Application.DTOs.Reader;

namespace SchoolLibrary.Application.DTOs.Reserve
{
    public record ReserveDto(
        int ReserveId,
        ReaderWithoutGradeDto Reader,
        string LibraryItem,
        DateTime ReservationDate
    );
}
