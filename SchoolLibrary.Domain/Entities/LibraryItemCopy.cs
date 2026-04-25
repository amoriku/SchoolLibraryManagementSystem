namespace SchoolLibrary.Domain.Entities
{
    public class LibraryItemCopy
    {
        // Is it counts as inventory code?
        public int Id { get; set; } 
        public string? InventoryCode { get; set; }

        public ItemCopyStatus Status { get; set; }

        public int LibraryItemId { get; set; }
        public LibraryItem LibraryItem { get; set; } = null!;

    }
}
