using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolLibrary.Domain.Entities;

namespace SchoolLibrary.Infrastructure.Configurations
{
    internal sealed class ItemAuthorConfiguration : IEntityTypeConfiguration<ItemAuthor>
    {
        public void Configure(EntityTypeBuilder<ItemAuthor> builder)
        {
            builder.HasKey(ia => ia.Id);

            builder
                .HasOne(ia => ia.LibraryItem)
                .WithMany(li => li.ItemAuthors)
                .HasForeignKey(ia => ia.LibraryItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(ia => ia.Author)
                .WithMany(a => a.ItemAuthors)
                .HasForeignKey(ia => ia.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
