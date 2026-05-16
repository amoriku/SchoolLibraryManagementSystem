using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SchoolLibrary.Domain.Interfaces;
using SchoolLibrary.Domain.ValueObjects;

namespace SchoolLibrary.Domain.Entities
{
    public class ApplicationUser : IdentityUser, ISoftDeletable
    {
        public FullName FullName { get; set; } = null!;

        public short? GradeId { get; set; }
        public Grade? Grade { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeletedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? LastModifiedAt { get; set; }

        public ICollection<UserHistory> UserHistories { get; set; } = [];
    }
}
