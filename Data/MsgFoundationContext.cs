using Microsoft.EntityFrameworkCore;
using MsgFoundation.Models;

namespace MsgFoundation.Data
{
    public class MsgFoundationContext : DbContext
    {
        public MsgFoundationContext(DbContextOptions<MsgFoundationContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Order> Orders => Set<Order>();
    }   
}
