using EduPlatform.Contexts;
using EduPlatform.Models;
using Microsoft.AspNetCore.Identity;

namespace EduPlatform.Initializer
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(AccountDbContext dbContext, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("Guest"));
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Student"));
            await roleManager.CreateAsync(new IdentityRole("Teacher"));
            await roleManager.CreateAsync(new IdentityRole("Moderator"));
            dbContext.SaveChanges();

            string login = "admin";
            string password = "Admin 1";
            if (await userManager.FindByNameAsync(login) == null)
            {
                UserModel admin = new UserModel
                {
                    UserName = login,
                    password = password
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
