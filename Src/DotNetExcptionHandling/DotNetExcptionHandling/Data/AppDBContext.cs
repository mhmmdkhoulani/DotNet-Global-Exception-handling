using DotNetExcptionHandling.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetExcptionHandling.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Driver> Drivers { get; set; }
    }
}
