using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.DTOs.Grades;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Infrastructure;

namespace SchoolLibrary.Application.Services
{
    public class GradeService : BaseService<GradeService>, IGradeService
    {
        public GradeService(AppDbContext context, ILogger<GradeService> logger) : base(context, logger) { }

        public async Task<List<GradeDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Grades
                .Select(g => new GradeDto(g.Id, g.DisplayName))
                .ToListAsync(cancellationToken);
        }

        public async Task<GradeDto> CreateAsync(CreateGradeDto dto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(dto.Letter) || dto.Number < 1 || dto.Number > 11)
            {
                throw new InvalidOperationException("Invalid grade number or letter");
            }

            string displayName = $"{dto.Number}-{dto.Letter}";

            bool gradeExists = await context.Grades
                .AnyAsync(
                    g => g.Number == dto.Number && g.Letter == dto.Letter, 
                    cancellationToken
                );

            if (gradeExists)
            {
                throw new AlreadyExistsException("Grade already exists");
            }

            Grade grade = new Grade
            {
                Number = dto.Number,
                Letter = dto.Letter,
                Users = Enumerable.Empty<ApplicationUser>().ToList<ApplicationUser>()
            };

            context.Grades.Add(grade);
            await context.SaveChangesAsync(cancellationToken);

            return new GradeDto(grade.Id, grade.DisplayName);
        }
    }
}
