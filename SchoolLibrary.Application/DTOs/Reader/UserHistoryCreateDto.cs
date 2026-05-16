using SchoolLibrary.Domain;

namespace SchoolLibrary.Application.DTOs.Reader
{
    public record UserHistoryCreateDto(string UserId, int LibraryItemCopyId, OperationType OperationType);
}
