using App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // 你也可以在这里加上自定义实体表
        public DbSet<Telescope> Telescope => Set<Telescope>();
    }

    // 下面是原来的 AppDbContext 类，
    //public class AppDbContext : DbContext
    //{
    //    public AppDbContext(DbContextOptions<AppDbContext> options)
    //        : base(options) { }

    //    public DbSet<User> Users => Set<User>();
    //    public DbSet<Telescope> Telescope => Set<Telescope>();

    //}
}
