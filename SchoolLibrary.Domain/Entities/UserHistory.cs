namespace SchoolLibrary.Domain.Entities
{
    public class UserHistory
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; } = DateTime.UtcNow;

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public int LibraryItemCopyId { get; set; }
        public LibraryItemCopy LibraryItemCopy { get; set; } = null!;
    }
}
