using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IStoreProductRepository : IGenericRepositories<StoreProduct>   
    {
        Store? GetStore(int id); 
        new Product? GetProduct(int id);
        OrderItem? GetOrderItem(int id);
    }
}
