namespace SchoolLibrary.Domain.Entities
{
    public class ItemAuthor
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public int LibraryItemId { get; set; }
        public LibraryItem LibraryItem { get; set; } = null!;
    }
}
