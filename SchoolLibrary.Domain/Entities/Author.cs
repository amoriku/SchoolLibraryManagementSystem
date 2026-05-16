using SchoolLibrary.Domain.ValueObjects;

namespace SchoolLibrary.Domain.Entities
{
    /// <summary>
    /// Сущность автора | Содержит справочные данные для запросов.
    /// </summary>
    public class Author
    {
        public int Id { get; set; }

        public FullName FullName { get; set; } = null!;
        public string? Pseudonym { get; set; }

        public ICollection<ItemAuthor> ItemAuthors { get; set; } = [];

    }
}
