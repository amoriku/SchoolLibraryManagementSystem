namespace SchoolLibrary.Domain.Entities
{
    public class Borrowing
    {
        public int Id { get; set; }

        public LibraryItemCopy LibraryItemCopy { get; set; } = null!;
        public int LibraryItemCopyId { get; set; }

        public ApplicationUser Reader { get; set; } = null!;
        public string ReaderId { get; set; } = string.Empty;


        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
}
