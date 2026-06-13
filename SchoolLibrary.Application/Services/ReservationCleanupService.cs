using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SchoolLibrary.Infrastructure;
using System.Resources;

namespace SchoolLibrary.Application.Services
{
    public class ReservationCleanupService : BackgroundService
    {
        private readonly ILogger<ReservationCleanupService> logger;
        private readonly IServiceProvider serviceProvider;
        private TimeSpan checkInterval = TimeSpan.FromHours(1); // Интервал проверки (например, каждый час)

        public ReservationCleanupService(
            IServiceProvider serviceProvider,
            ILogger<ReservationCleanupService> logger
            )
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"{this.ToString()} Starting the background reservation cleanup service");

            while (!stoppingToken.IsCancellationRequested) 
            {
                try
                {
                    await ProcessExpiredReservationAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError($"{this.ToString()} An error occurred while processing expired reservations: {ex.Message}");
                }

                await Task.Delay(checkInterval, stoppingToken);
           
            }

        }

        private async Task ProcessExpiredReservationAsync()
        {
            using var scope = serviceProvider.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var expirationThreshold = DateTime.UtcNow.AddDays(-2); // Например, удалять бронирования, которые были созданы более 2 дней назад

            // Поиск потенциально просроченных броней (читатель не пришел за книгой)
            var expiredReservations = await context.Reservations
                .Where(r => r.IsActive && r.ReservedAt < expirationThreshold)
                .ToListAsync();

            if (!expiredReservations.Any()) return;

            logger.LogInformation($"{this.ToString()} found potentially {expiredReservations.Count()} expired reservations");
            
            foreach (var expiredReservation in expiredReservations) 
            {
                // Сколько людей забронировало эту книгу перед текущим пользователем
                int peopleAhead = await context.Reservations
                    .CountAsync(r => r.IsActive
                                     && r.LibraryItemId == expiredReservation.LibraryItemId
                                     && r.ReservedAt < expiredReservation.ReservedAt);

                // Сколько всего физических экземпляров сейчас есть в библиотеке
                int availableCopiesCount = await context.LibraryItemCopies
                    .CountAsync(ic => ic.LibraryItemId == expiredReservation.LibraryItemId
                                      && ic.Status == Domain.ItemCopyStatus.Available);

                if (availableCopiesCount > peopleAhead)
                {
                    expiredReservation.IsActive = false;

                    context.UserHistories.Add(new Domain.Entities.UserHistory
                    {
                        Date = DateTime.UtcNow,
                        OperationType = Domain.OperationType.ReserveExpired,
                        UserId = expiredReservation.ReaderId,
                        LibraryItemCopyId = await context.LibraryItemCopies
                            .Where(ic => ic.LibraryItemId == expiredReservation.LibraryItemId)
                            .Select(ic => ic.Id)
                            .FirstOrDefaultAsync()
                    });
                }

                logger.LogInformation($"{this.ToString()} reservation {expiredReservation.Id} was canceled for reader {expiredReservation.ReaderId}");
            }
            await context.SaveChangesAsync();
        }
    }
}
