using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Domain.ValueObjects;
using SchoolLibrary.Infrastructure.Configurations;

namespace SchoolLibrary.Infrastructure
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; } 
        public DbSet<Author> Authors { get; set; }
        public DbSet<LibraryItem> LibraryItems { get; set; }
        public DbSet<LibraryItemCopy> LibraryItemCopies { get; set; }
        public DbSet<ItemAuthor> ItemAuthors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<UserHistory> UserHistories { get; set; }
        public DbSet<Fund> Funds { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ItemAuthorConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationUserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GradeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserHistoryConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryItemCopyConfiguration).Assembly);            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FullNameConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RefreshTokenConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReservationConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BorrowingConfiguration).Assembly);
        }
    }
}
