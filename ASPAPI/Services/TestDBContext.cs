using ASPAPI.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace ASPAPI.Services {
    public class TestDBContext: DbContext {
        public TestDBContext(DbContextOptions<TestDBContext> options) : base(options) { }
        public TestDBContext() { }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Store> Stores { get; set; }    
    }
}
