using Microsoft.EntityFrameworkCore;
using WebServer.Models;

namespace WebServer.Data
{
    public class WebServerDbContext : DbContext
    {
        public WebServerDbContext(DbContextOptions<WebServerDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}
