using EduPlatform.Contexts;
using EduPlatform.Initializer;
using EduPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AccountDbContext>(options => options.
            UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;" +
            "Initial Catalog=LearnPlatform;" +
            "Integrated Security=True;Connect Timeout=30;Encrypt=False;" +
            "Trust Server Certificate=False;Application Intent=ReadWrite;" +
            "Multi Subnet Failover=False"));

            builder.Services.AddIdentity<UserModel, IdentityRole>()
                .AddEntityFrameworkStores<AccountDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    //var context = services.GetRequiredService<ContextFactory>();
                    var accountContext = services.GetRequiredService<AccountDbContext>();
                    var userManager = services.GetRequiredService<UserManager<UserModel>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    DbInitializer.InitializeAsync(accountContext, userManager, rolesManager).Wait();

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ошибка при инициализации базы данных.");
                }
            }

            app.Run();
        }
    }
}
