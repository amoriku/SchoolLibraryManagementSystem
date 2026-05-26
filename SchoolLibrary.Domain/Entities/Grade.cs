namespace SchoolLibrary.Domain.Entities
{
    /// <summary>
    /// Сущность класса читателя (Например: "5-Б") || Содержит справочные данные для запросов.
    /// </summary>
    public class Grade
    {
        public short Id { get; set; }

        public byte Number { get; set; }
        public string Letter { get; set; } = string.Empty;

        public string DisplayName => $"{Number}-{Letter}";

        public ICollection<ApplicationUser> Users { get; set; } = [];
    }
}
