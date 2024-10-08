using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<FastFoodItem> FastFoodItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Invoice> invoices { get; set; }

    }
}
