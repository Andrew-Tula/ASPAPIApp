using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Repositories
{
    public class StoreProductRepository : GenericRepository<StoreProduct>, IStoreProductRepository
    {
        public StoreProductRepository(TestDBContext dbContext) : base(dbContext) { }
//        public User? GetUser(int id) => dbContext.Users.FirstOrDefault(u => u.Id == id);
        public Store? GetStore(int id) => dbContext.Stores.FirstOrDefault(s => s.Id == id);
        public new Product? GetProduct(int id) => dbContext.Products.FirstOrDefault(p => p.Id == id);
        public OrderItem? GetOrderItem(int id) => dbContext.OrderItems.FirstOrDefault(o => o.Id == id);
    }
}
