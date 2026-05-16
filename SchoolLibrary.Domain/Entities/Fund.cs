namespace SchoolLibrary.Domain.Entities
{
    /// <summary>
    /// Сущность библиотечных фондов | Содержит справочные данные для запросов.
    /// </summary>
    public class Fund
    {
        public short Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
