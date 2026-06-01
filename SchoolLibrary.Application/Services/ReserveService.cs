using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.DTOs.Reader;
using SchoolLibrary.Application.DTOs.Reserve;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Infrastructure;

namespace SchoolLibrary.Application.Services
{
    public class ReserveService : BaseService<ReserveService>, IReserveService
    {
        private readonly IAuthService authService;

        public ReserveService(
            AppDbContext context, 
            ILogger<ReserveService> logger,
            IAuthService authService
        ) 
            : base(context, logger) 
        {
            this.authService = authService;
        }

        public async Task<List<ReserveDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var reserveQuery = context.Reservations.AsNoTracking();

            var reservations = await reserveQuery
                .Include(r => r.Reader)
                .Where(r => r.IsActive)
                .Select(r => new ReserveDto(
                    r.Id,
                    new ReaderWithourGradeDto(
                        r.ReaderId, 
                        r.Reader.FullName.FirstName,
                        r.Reader.FullName.LastName,
                        r.Reader.FullName.MiddleName
                    ),
                    r.LibraryItem.Title,
                    r.ReservedAt
                ))
                .ToListAsync(cancellationToken);

            return reservations;
        }

        public async Task CreateAsync(ReserveCreateDto dto, CancellationToken cancellationToken)
        {
            var book = await context.LibraryItems
                .FirstOrDefaultAsync(li => li.Id == dto.LibraryItemId, cancellationToken);
            var currentUser = await authService.GetCurrentUserAsync(cancellationToken);

            if (book == null || currentUser == null)
            {
                throw new InvalidOperationException("Invalid user or book");
            }

            var oldReservation = await context.Reservations
                .FirstOrDefaultAsync(r => r.LibraryItemId == dto.LibraryItemId && r.ReaderId == currentUser.Id);

            if (oldReservation != null)
            {
                throw new InvalidOperationException("You are already have an reservation on this book");
            }

            var newReservation = new Reservation
            {
                LibraryItemId = dto.LibraryItemId,
                ReaderId = currentUser.Id,
                IsActive = true,
                ReservedAt = DateTime.UtcNow
            };

            context.Reservations.Add(newReservation);
            await context.SaveChangesAsync();
        }
    }
}
