using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.DTOs.Fund;
using SchoolLibrary.Application.Exceptions;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Infrastructure;

namespace SchoolLibrary.Application.Services
{
    public class FundService : BaseService<FundService>, IFundService
    {
        public FundService(AppDbContext context, ILogger<FundService> logger) : base(context, logger) { }

        public async Task<Fund> CreateAsync(FundCreateDto dto, CancellationToken cancellationToken)
        {
            bool exists = await context.Funds
                .AnyAsync(f => f.Name == dto.Name, cancellationToken);

            if (exists)
            {
                throw new AlreadyExistsException($"Fund {dto.Name} already exists");
            }

            Fund fund = new Fund { Name = dto.Name };

            context.Funds.Add(fund);
            await context.SaveChangesAsync(cancellationToken);

            return fund;
        }

        public async Task<Fund?> GetByIdAsync(short id, CancellationToken cancellationToken)
        {
            var fund = await context.Funds
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

            if (fund is null)
            {
                throw new NotFoundException("Fund not found");
            }

            return fund;
        }

        public async Task<List<Fund>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await context.Funds.ToListAsync(cancellationToken);
        }
    }
}
