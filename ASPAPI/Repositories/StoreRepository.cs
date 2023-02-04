using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace ASPAPI.Repositories
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        public StoreRepository(TestDBContext dbContext) : base(dbContext) { }


        public Product? GetProduct(int id) => dbContext.Products.FirstOrDefault(p => p.Id == id);
      
        public OrderItem? GetOrderItem(int id) => dbContext.OrderItems.FirstOrDefault(o => o.Id == id);
    }
}
