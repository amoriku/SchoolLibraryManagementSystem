using SchoolLibrary.Domain;

namespace SchoolLibrary.Application.DTOs.Reader
{
    public record CreateReaderHistoryDto(string ReaderId, int LibraryItemCopyId, OperationType OperationType);
}
