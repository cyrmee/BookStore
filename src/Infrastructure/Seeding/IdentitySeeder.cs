using Domain.Models;
using Domain.Types;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Infrastructure.Seeding;

public class IdentitySeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
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

    public static async Task SeedUsers(UserManager<User> userManager)
    {
        var adminUserName = "admin";
        var adminEmail = "admin@example.com";
        var adminPassword = "Admin1.";

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

        var managerUserName = "manager";
        var managerEmail = "manager@example.com";
        var managerPassword = "Manager1.";

        var managerUser = await userManager.FindByEmailAsync(managerEmail);
        if (managerUser == null)
        {
            managerUser = new User
            {
                UserName = managerUserName,
                Email = managerEmail
            };

            await userManager.CreateAsync(managerUser, managerPassword);
            await userManager.AddToRoleAsync(managerUser, UserRole.Manager);
        }

        var customerUserName = "customer";
        var customerEmail = "customer@example.com";
        var customerPassword = "Customer1.";

        var customerUser = await userManager.FindByEmailAsync(customerEmail);
        if (customerUser == null)
        {
            customerUser = new User
            {
                UserName = customerUserName,
                Email = customerEmail
            };

            await userManager.CreateAsync(customerUser, customerPassword);
            await userManager.AddToRoleAsync(customerUser, UserRole.Customer);
        }
    }
}
