using Microsoft.EntityFrameworkCore;
using AmarraderoLlanero.Models;

namespace AmarraderoLlanero.Data
{
    public class AmarraderoLlaneroContext : DbContext
    {
        public AmarraderoLlaneroContext(DbContextOptions<AmarraderoLlaneroContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Order> Orders => Set<Order>();
    }   
}
