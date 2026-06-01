using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Infrastructure.Configurations
{
    internal sealed class BorrowingConfiguration : IEntityTypeConfiguration<Borrowing>
    {
        public void Configure(EntityTypeBuilder<Borrowing> builder)
        {
            builder.HasKey(b => b.Id);

            builder
                .HasOne(b => b.Reader)
                .WithMany()
                .HasForeignKey(b => b.ReaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.LibraryItemCopy)
                .WithMany(ic => ic.Borrowings)
                .HasForeignKey(b => b.LibraryItemCopyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
