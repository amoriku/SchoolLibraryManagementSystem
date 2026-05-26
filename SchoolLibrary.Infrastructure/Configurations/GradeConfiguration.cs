using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Infrastructure.Configurations
{
    internal sealed class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.HasKey(g => g.Id);

            builder
                .Property(g => g.Letter)
                .IsRequired()
                .HasMaxLength(2);

            builder.Ignore(g => g.DisplayName);

        }
    }
}
