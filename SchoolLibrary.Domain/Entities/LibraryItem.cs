using SchoolLibrary.Domain.Interfaces;
using System.ComponentModel;

namespace SchoolLibrary.Domain.Entities
{
    public class LibraryItem : ISoftDeletable
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? PublishedYear { get; set; }
        public decimal? Price { get; set; } = 0;

        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeletedAt { get; set; }
        public DateTime ReceiptDate { get; set; } = DateTime.UtcNow;

        public ICollection<ItemAuthor> ItemAuthors { get; set; } = [];
        public ICollection<LibraryItemCopy> LibraryItemCopies { get; set; } = [];
        public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
