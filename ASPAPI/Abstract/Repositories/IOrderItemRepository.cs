using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>    
    {
        new Product? GetProduct(int id);
        new Order? GetOrder(int id);
    }
}