using SchoolLibrary.Application.DTOs.Grades;

namespace SchoolLibrary.Application.Interfaces
{
    public interface IGradeService 
    {
        Task<List<GradeDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<GradeDto> CreateAsync(CreateGradeDto dto, CancellationToken cancellationToken);
    }
}
