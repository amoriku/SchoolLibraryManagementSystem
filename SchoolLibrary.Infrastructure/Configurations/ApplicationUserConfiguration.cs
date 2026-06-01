using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Infrastructure.Configurations
{
    internal sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(au => au.Id);

            builder.ComplexProperty(au => au.FullName);

            builder
                .HasOne(au => au.Grade)
                .WithMany(g => g.Users)
                .HasForeignKey(au => au.GradeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
