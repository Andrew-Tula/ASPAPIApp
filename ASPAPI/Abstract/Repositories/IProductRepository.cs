using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IProductRepository : IGenericRepositories<Product>
    {
        Product? GetProduct(int id);

       // Product? Remove(Product item); 
    }
}