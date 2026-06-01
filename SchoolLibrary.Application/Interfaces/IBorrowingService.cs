using SchoolLibrary.Application.DTOs.Borrow;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IBorrowingService
    {
        Task<BorrowingDto> CreateAsync(CreateBorrowingDto dto, CancellationToken cancellationToken);
        Task<List<BorrowingDto>> GetAllAsync(CancellationToken cancellationToken);
    }
}
