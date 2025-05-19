using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using DreamDay.Models;

namespace DreamDay.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Check if the "Admin" role exists, if not, create it
            var roleExist = await roleManager.RoleExistsAsync("Admin");
            if (!roleExist)
            {
                var role = new IdentityRole("Admin");
                await roleManager.CreateAsync(role);
            }

            // Check if the Admin user exists, if not, create it
            var adminUser = await userManager.FindByEmailAsync("admin@dreamday.com");

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@dreamday.com",
                    Email = "admin@dreamday.com",
                    FullName = "Admin User"
                };

                var result = await userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    // Add the user to the Admin role
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
