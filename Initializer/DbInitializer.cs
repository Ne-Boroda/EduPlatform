using EduPlatform.Contexts;
using EduPlatform.Models;
using Microsoft.AspNetCore.Identity;

namespace EduPlatform.Initializer
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(AccountDbContext dbContext, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            dbContext.SaveChanges();

            string login = "admin";
            string password = "Admin 1";
            if (await userManager.FindByNameAsync(login) == null)
            {
                UserModel admin = new UserModel
                {
                    email = login,
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
