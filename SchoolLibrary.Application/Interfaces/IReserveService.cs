using SchoolLibrary.Application.DTOs.Reserve;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IReserveService 
    {
        Task CreateAsync(ReserveCreateDto dto, CancellationToken cancellationToken);
        Task<List<ReserveDto>> GetAllAsync(CancellationToken cancellationToken);
    }
}
