using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(TestDBContext dbContext) : base(dbContext) { }

        public new Product? GetProduct(int id) => dbContext.Products.FirstOrDefault(p => p.Id == id);
        public new Order? GetOrder(int id) => dbContext.Orders.FirstOrDefault(o => o.Id == id);
    }
}
