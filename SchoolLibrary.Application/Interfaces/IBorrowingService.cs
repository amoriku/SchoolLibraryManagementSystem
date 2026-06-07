using SchoolLibrary.Application.DTOs.Borrow;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IBorrowingService
    {
        Task<BorrowingDto> CreateAsync(CreateBorrowingDto dto, CancellationToken cancellationToken);
        Task<List<BorrowingDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<BorrowingDto>> GetActiveBorrowings(CancellationToken cancellationToken);
        Task<bool> ReturnAsync(int itemCopyId, CancellationToken cancellationToken);
    }
}
