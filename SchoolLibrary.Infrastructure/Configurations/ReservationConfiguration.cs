using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Infrastructure.Configurations
{
    internal sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(r => r.Id);

            builder
                .HasOne(r => r.LibraryItem)
                .WithMany(ic => ic.Reservations)
                .HasForeignKey(r => r.LibraryItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(r => r.Reader)
                .WithMany()
                .HasForeignKey(r => r.ReaderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
