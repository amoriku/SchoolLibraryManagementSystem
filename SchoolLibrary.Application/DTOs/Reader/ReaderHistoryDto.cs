using SchoolLibrary.Domain;

namespace SchoolLibrary.Application.DTOs.Reader
{
    public record ReaderHistoryDto(string ReaderId, string LibraryItemTitle, DateTime Date, string OperationType);
}
