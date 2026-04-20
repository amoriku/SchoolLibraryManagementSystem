namespace SchoolLibrary.Domain.Interfaces
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; }
        DateTimeOffset? DeletedAt { get; }
    }
}
