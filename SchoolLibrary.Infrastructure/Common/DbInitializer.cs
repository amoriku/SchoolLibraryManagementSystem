using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Domain;
using SchoolLibrary.Domain.Constants;
using SchoolLibrary.Domain.Entities;
using SchoolLibrary.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

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

        public void Initialize() 
        {
            try
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    Console.WriteLine("[Infrastructure:DbInitializer] Applying pending migratoins");
                    context.Database.Migrate();
                }
                //else if (context.Database.CanConnect()
                //    && context.Database.)
                //{
                //    Console.WriteLine("[Infrastructure:DbInitializer] Creating Database...");
                //    //context.Database.Migrate();
                //}

                SeedData().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Infrastructure] error when initializing database: \n{ex}");
                throw;
            }
        }

        private async Task SeedData()
        {
            if (!context.Grades.Any())
            {
                var defaultGrade = new Grade { Id = 1, Name = "L-A" };
                context.Grades.Add(defaultGrade);
                await context.SaveChangesAsync();
            }

            List<string> roles = new List<string>()
            {
                UserRoles.Admin,
                UserRoles.Librarian,
                UserRoles.Reader,
                UserRoles.Guest,
            };

            foreach (var role in roles)
            {
                var roleName = role;
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


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
                    await userManager.AddToRoleAsync(adminUser, UserRoles.Admin.ToString());
                    Console.WriteLine($"[Infrastructure:DbInitializer] Successfully created {adminUser.UserName}");
                }
            }
        }
    }
}
