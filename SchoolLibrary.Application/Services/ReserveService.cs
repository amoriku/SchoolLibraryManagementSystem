using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Application.DTOs.Reader;
using SchoolLibrary.Application.DTOs.Reserve;
using SchoolLibrary.Application.Interfaces;
using SchoolLibrary.Domain;
using SchoolLibrary.Domain.Constants;
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


        // Получение активных броней читателя (для читателя)
        public async Task<List<ReserveWithoutReaderDto>> GetActiveAsync(CancellationToken cancellationToken)
        {
            var user = await authService.GetCurrentUserAsync(cancellationToken);
            var reservations = await context.Reservations
                .Where(r => r.ReaderId == user.Id && r.IsActive)
                .Select(r => new ReserveWithoutReaderDto(
                    r.Id,
                    r.LibraryItem.Title,
                    r.ReservedAt
                ))
                .ToListAsync(cancellationToken);

            return reservations;
        }

        // Отмена брони читателем
        public async Task<ReserveWithoutReaderDto> CancelAsync(int reserveId, CancellationToken cancellationToken)
        {
            var user = await authService.GetCurrentUserAsync(cancellationToken);
            if (user == null)
            {
                throw new InvalidOperationException("User is not authorized");
            }

            bool isStaff = user.Role == UserRoles.Librarian || user.Role == UserRoles.Admin;

            var query = context.Reservations
                .Include(r => r.LibraryItem)
                .Where(r => r.Id == reserveId && r.IsActive);

            if (!isStaff)
            {
                query = query.Where(r => r.ReaderId == user.Id);
            }

            var reservationToCancel = await query.FirstOrDefaultAsync(cancellationToken);

            if (reservationToCancel == null)
            {
                logger.LogWarning("Reservation {ReserveId} not found or already inactive. IsStaff: {IsStaff}", reserveId, isStaff);
                throw new InvalidOperationException($"Reservation {reserveId} does not exist, is already inactive, or you don't have access.");
            }

            reservationToCancel.IsActive = false;
            await context.SaveChangesAsync(cancellationToken);

            return new ReserveWithoutReaderDto(
                reservationToCancel.Id,
                reservationToCancel.LibraryItem.Title,
                reservationToCancel.ReservedAt
            );
        }

        // Получение всех броней (за все время)
        public async Task<List<ReserveDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var reserveQuery = context.Reservations.AsNoTracking();

            var reservations = await reserveQuery
                .Include(r => r.Reader)
                .Where(r => r.IsActive)
                .Select(r => new ReserveDto(
                    r.Id,
                    new ReaderWithoutGradeDto(
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

        // Создание записи брони
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
                .FirstOrDefaultAsync(r => r.LibraryItemId == dto.LibraryItemId && r.ReaderId == currentUser.Id && r.IsActive);

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
