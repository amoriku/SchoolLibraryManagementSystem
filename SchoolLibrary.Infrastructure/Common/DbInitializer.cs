using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Domain.Constants;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Domain.ValueObjects;

namespace SchoolLibrary.Infrastructure.Common
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDbContext context;

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

        public async Task Initialize() 
        {
            int retryCount = 6;
            bool isDbReady = false;

            while (!isDbReady && retryCount > 0)
            {
                try
                {
                    Console.WriteLine($"[DbInitializer] Checking pending migrations... (Attempts left: {retryCount})");

                    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                    if (pendingMigrations.Any())
                    {
                        Console.WriteLine("[DbInitializer] Applying pending migrations...");
                        await context.Database.MigrateAsync();
                    }

                    isDbReady = true; 
                }
                catch (Exception ex)
                {
                    retryCount--;
                    Console.WriteLine($"[DbInitializer] Database is NOT ready yet: {ex.Message}");

                    if (retryCount == 0)
                    {
                        Console.WriteLine("[DbInitializer] CRITICAL: Could not connect to the database. Exiting.");
                        throw; 
                    }

                    Console.WriteLine("[DbInitializer] Waiting 5 seconds before retrying...");
                    await Task.Delay(5000); 
                }
            }

            Console.WriteLine("[DbInitializer] Starting SeedData...");
            await SeedData();
        }

        // Добавление первичных данных в базу данных.
        private async Task SeedData()
        {
            // Создание первичного класса для администратора L-A (Library-Admin).
            if (!await context.Grades.AnyAsync())
            {
                var defaultGrade = new Grade { Id = 1, Letter = "ША", Number = 0 };
                context.Grades.Add(defaultGrade);
                await context.SaveChangesAsync();
                Console.WriteLine("[DbInitializer] Default grade created.");
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
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    Console.WriteLine($"[DbInitializer] Role {role} created.");
                }
            }

            // Создание первичного пользователя в системе (Администратора)
            if (!await userManager.Users.AnyAsync())
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "admin@oc.com",
                    FullName = new FullName("Mark", "Shangin"),
                    EmailConfirmed = true,
                    GradeId = 1,
                };

                var result = await userManager.CreateAsync(adminUser, "12345Aa%");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                    Console.WriteLine($"[DbInitializer] Successfully created {adminUser.UserName}");
                }
                else
                {
                    Console.WriteLine($"[DbInitializer] ERROR creating admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
