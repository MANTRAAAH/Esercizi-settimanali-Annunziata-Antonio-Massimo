using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PizzeriaS7.Models;
using System;
using System.Threading.Tasks;

namespace PizzeriaS7.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Utente>>();

            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var admin = new Utente
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                Nome = "Admin"
            };

            string adminPassword = "Admin@123";
            var _user = await userManager.FindByEmailAsync(admin.Email);

            if (_user == null)
            {
                var createAdmin = await userManager.CreateAsync(admin, adminPassword);
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
            else
            {
               
                if (!await userManager.IsInRoleAsync(_user, "Admin"))
                {
                    await userManager.AddToRoleAsync(_user, "Admin");
                }
            }
        }
    }
}
