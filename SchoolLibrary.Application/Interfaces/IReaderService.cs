using SchoolLibrary.Application.DTOs.Reader;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IReaderService
    {
        // Loan (Borrowing), Return, History
        Task<ReaderHistoryDto> CreateReaderHistoryAsync(CreateReaderHistoryDto dto, CancellationToken cancellationToken);
        Task<List<ReaderHistoryDto>> GetReaderHistoryAsync(string readerId, CancellationToken cancellationToken);
        Task<List<ReaderDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<ReaderDto> CreateAsync(CreateReaderDto dto, CancellationToken cancellationToken);
    }
}
