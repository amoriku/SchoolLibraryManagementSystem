using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Infrastructure.Configurations
{
    internal sealed class LibraryItemConfiguration : IEntityTypeConfiguration<LibraryItem>
    {
        public void Configure(EntityTypeBuilder<LibraryItem> builder)
        {
            builder.HasKey(li => li.Id);

            builder
                .Property(li => li.Title)
                .HasMaxLength(255);
        }
    }
}
