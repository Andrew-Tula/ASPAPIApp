using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order> 
    {
        User? GetUser(int id);
        new Product? GetProduct(int id);
    }
}