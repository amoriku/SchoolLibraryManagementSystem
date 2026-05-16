using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Infrastructure.Configurations
{
    internal sealed class UserHistoryConfiguration : IEntityTypeConfiguration<UserHistory>
    {
        public void Configure(EntityTypeBuilder<UserHistory> builder)
        {
            builder.HasKey(uh => uh.Id);

            builder
                .HasOne(uh => uh.User)
                .WithMany(au => au.UserHistories)
                .HasForeignKey(uh => uh.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(uh => uh.LibraryItemCopy)
                .WithMany(ic => ic.UserHistories)
                .HasForeignKey(uh => uh.LibraryItemCopyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(uh => uh.OperationType)
                .HasConversion<string>();
        }
    }


}
