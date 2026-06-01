namespace SchoolLibrary.Domain.Entities
{
    public class LibraryItemCopy
    {
        public int Id { get; set; } 
        public string? InventoryCode { get; set; }

        public ItemCopyStatus Status { get; set; }

        public short FundId { get; set; }
        public Fund Fund { get; set; } = null!;

        public int LibraryItemId { get; set; }
        public LibraryItem LibraryItem { get; set; } = null!;

        public ICollection<UserHistory> UserHistories { get; set; } = [];
        public ICollection<Borrowing> Borrowings { get; set; } = [];

    }
}
