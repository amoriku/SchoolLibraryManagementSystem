using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLibrary.Domain.ValueObjects;

namespace SchoolLibrary.Infrastructure.Configurations
{
    internal sealed class FullNameConfiguration : IEntityTypeConfiguration<FullName>
    {
        public void Configure(EntityTypeBuilder<FullName> builder)
        {
            builder.HasNoKey();

            builder
                .Property(fn => fn.FirstName)
                .HasMaxLength(255);

            builder
                .Property(fn => fn.LastName)
                .HasMaxLength(255);

            builder
                .Property(fn => fn.MiddleName)
                .HasMaxLength(255);

        }
    }
}
