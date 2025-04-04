using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EduPlatform.Models;

namespace EduPlatform.Contexts
{
    public class AccountDbContext : IdentityDbContext<UserModel>
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
            : base(options) { }
    }
}
