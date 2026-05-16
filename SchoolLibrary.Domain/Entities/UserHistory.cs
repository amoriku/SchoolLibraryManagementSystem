namespace SchoolLibrary.Domain.Entities
{
    public class UserHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public int LibraryItemCopyId { get; set; }
        public LibraryItemCopy LibraryItemCopy { get; set; } = null!;

        public OperationType OperationType { get; set; } 
    }
}
