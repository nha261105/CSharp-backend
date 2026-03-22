using InteractHub.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace InteractHub.Infrastructure.Data.Seeders;

public static class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        string[] roles = {"Admin", "Moderator", "User"};
        foreach(var r in roles)
        {
            if(!await roleManager.RoleExistsAsync(r))
            {
                await roleManager.CreateAsync(new Role{Name = r, Description = r switch
                {
                    "Admin" => "Quản trị viên",
                    "Moderator" => "Kiểm duyệt viên",
                    "User" => "Người dùng",
                    _ => ""
                }
                });
            }
        }

        const string adminMail = "admin@test.com";
        if(await userManager.FindByEmailAsync(adminMail) == null)
        {
            var admin = new User
            {
                UserName = "admin",
                Email = adminMail,
                Fullname = "Admin System",
                EmailConfirmed = true,
                IsActive = true
            };

            var res = await userManager.CreateAsync(admin,"admin@12345");
            if(res.Succeeded)
            {
                await userManager.AddToRoleAsync(admin,"Admin");
            }
        }
    }
}