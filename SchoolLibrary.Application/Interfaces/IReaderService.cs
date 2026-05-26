using SchoolLibrary.Application.DTOs.Reader;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IReaderService
    {
        // Loan (Borrowing), Return, History
        Task<UserHistory> CreateUserHistoryRecordAsync(UserHistoryCreateDto dto, CancellationToken cancellationToken);
        Task<UserHistory?> GetUserHistoryAsync(string userId, CancellationToken cancellationToken);
        Task<List<ReaderDto>> GetAllAsync(CancellationToken cancellationToken);
    }
}
