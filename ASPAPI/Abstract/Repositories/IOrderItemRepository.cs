using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IOrderItemRepository : IGenericRepositories<OrderItem>    
    {
        new Product? GetProduct(int id);
        new Order? GetOrder(int id);
    }
}