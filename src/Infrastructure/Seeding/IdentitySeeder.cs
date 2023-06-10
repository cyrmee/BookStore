using Domain.Models;
using Domain.Types;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Infrastructure.Seeding;

public class IdentitySeeder
{
    public static async Task SeedAspNetRoles(RoleManager<IdentityRole> roleManager)
    {
        var roles = new List<IdentityRole>
        {
            new IdentityRole { Name = UserRole.Admin },
            new IdentityRole { Name = UserRole.Manager },
            new IdentityRole { Name = UserRole.Customer }
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    public static async Task SeedAdminUser(UserManager<User> userManager)
    {
        var adminUserName = "admin";
        var adminEmail = "admin@example.com";
        var adminPassword = "Mercyrme1.";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminUserName,
                Email = adminEmail
            };

            await userManager.CreateAsync(adminUser, adminPassword);
            await userManager.AddToRoleAsync(adminUser, UserRole.Admin);
        }
    }
}
