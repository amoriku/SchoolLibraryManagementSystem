namespace SchoolLibrary.Domain.Entities
{
    public class Book : LibraryItem
    {
        public string? ISBN_13 { get; set; } = string.Empty;
        public string? ISBN_10 { get; set; }

        public string? Edition { get; set; } = string.Empty;
    }
}
