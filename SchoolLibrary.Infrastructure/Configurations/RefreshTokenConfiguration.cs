using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Infrastructure.Configurations
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder
                .Property(rt => rt.Token)
                .HasMaxLength(128);

            builder
                .HasIndex(rt => rt.Token)
                .IsUnique();

            // One token per user session (theres an history included)
            builder
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
