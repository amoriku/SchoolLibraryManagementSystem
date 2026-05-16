namespace SchoolLibrary.Domain.Entities
{
    /// <summary>
    /// Сущность класса читателя (Например: "5-Б") || Содержит справочные данные для запросов.
    /// </summary>
    public class Grade
    {
        public short Id { get; set; }

        public string Name { get; set; } = string.Empty; 

        public ICollection<ApplicationUser> Users { get; set; } = [];
    }
}
