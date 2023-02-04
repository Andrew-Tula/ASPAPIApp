using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IStoreRepository : IGenericRepositories<Store>
    {
     Product? GetProduct(int id);
     OrderItem? GetOrderItem(int id);
    }
}