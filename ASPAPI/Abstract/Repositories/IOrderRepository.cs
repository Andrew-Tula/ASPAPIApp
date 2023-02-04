using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IOrderRepository : IGenericRepositories<Order> 
    {
        User? GetUser(int id);
    }
}