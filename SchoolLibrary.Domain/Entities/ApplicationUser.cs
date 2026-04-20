using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SchoolLibrary.Domain.Interfaces;

namespace SchoolLibrary.Domain.Entities
{
    public class ApplicationUser : IdentityUser, ISoftDeletable
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }


        public bool IsDeleted { get; } = false;
        public DateTimeOffset? DeletedAt { get; }
    }
}
