namespace SchoolLibrary.Domain.Entities
{
    // User grade entity | Сущность класса пользователя  
    public class Grade
    {
        public short Id { get; set; }

        // For example: "4-A" | Например: "4-А"
        public string Name { get; set; } = string.Empty; 

        public ICollection<ApplicationUser> Users { get; set; } = [];
    }
}
