using SchoolLibrary.Domain.Interfaces;

namespace SchoolLibrary.Domain.Entities
{
    public class LibraryItem : ISoftDeletable
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? PublishedYear { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeletedAt { get; set; }

        public ICollection<ItemAuthor> ItemAuthors { get; set; } = [];
        public ICollection<LibraryItemCopy> LibraryItemCopies { get; set; } = [];
    }
}
