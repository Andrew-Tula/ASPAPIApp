using ASPAPI.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace ASPAPI.Services {
    public class TestDBContext: DbContext {
        public TestDBContext(DbContextOptions<TestDBContext> options) : base(options) { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
    }

}
