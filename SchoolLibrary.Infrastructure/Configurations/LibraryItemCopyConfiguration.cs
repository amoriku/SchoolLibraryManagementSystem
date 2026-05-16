using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Infrastructure.Configurations
{
    internal sealed class LibraryItemCopyConfiguration : IEntityTypeConfiguration<LibraryItemCopy>
    { 
        public void Configure(EntityTypeBuilder<LibraryItemCopy> builder)
        {
            builder.HasKey(ic => ic.Id);

            builder
               .HasOne(ic => ic.LibraryItem)
               .WithMany(li => li.LibraryItemCopies)
               .HasForeignKey(ic => ic.LibraryItemId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
               .Property(ic => ic.Status)
               .HasConversion<string>();
        }
    }
}
