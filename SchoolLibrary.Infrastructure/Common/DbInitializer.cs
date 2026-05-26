using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Domain.Constants;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Domain.ValueObjects;

namespace SchoolLibrary.Infrastructure.Common
{
    public class DbInitializer : IDbInitializer
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private AppDbContext context;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }

        // Первичная инициализая базы данных.
        public void Initialize() 
        {
            try
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    Console.WriteLine("[Infrastructure:DbInitializer] Applying pending migratoins");
                    context.Database.Migrate();
                }
                SeedData().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Infrastructure] error when initializing database: \n{ex}");
                throw;
            }
        }

        // Добавление первичных данных в базу данных.
        private async Task SeedData()
        {
            // Создание первичного класса для администратора L-A (Library-Admin).
            if (!context.Grades.Any())
            {
                var defaultGrade = new Grade { Id = 1, Letter = "ША", Number = 0 };
                context.Grades.Add(defaultGrade);
                await context.SaveChangesAsync();
            }

            // Ввод первичных ролей.
            List<string> roles = new List<string>()
            {
                UserRoles.Admin,
                UserRoles.Librarian,
                UserRoles.Reader,
                UserRoles.Guest,
            };

            // Добавление ролей в базу данных.
            foreach (var role in roles)
            {
                var roleName = role;
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Создание первичного пользователя в системе (Администратора)
            if (!userManager.Users.Any())
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "admin@oc.com",
                    FullName = new FullName("Mark", "Shangin"),
                    EmailConfirmed = true,
                    GradeId = 1,
                };

                var result = await userManager.CreateAsync(adminUser);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                    Console.WriteLine($"[Infrastructure:DbInitializer] Successfully created {adminUser.UserName}");
                }
            }
        }
    }
}
