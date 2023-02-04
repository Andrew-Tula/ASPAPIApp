using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IOrderItemRepository : IGenericRepositories<OrderItem>    
    {
        Product? GetProduct(int id);
        Order? GetOrder(int id);
    }
}