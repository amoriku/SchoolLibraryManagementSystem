using SchoolLibrary.Application.DTOs.Fund;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IFundService
    {
        Task<Fund> CreateAsync(FundCreateDto dto, CancellationToken cancellationToken);
        Task<Fund?> GetByIdAsync(short id, CancellationToken cancellationToken);
        Task<List<Fund>> GetAllAsync(CancellationToken cancellationToken);
    }
}
