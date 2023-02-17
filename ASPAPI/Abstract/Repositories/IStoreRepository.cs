using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IStoreRepository : IGenericRepositories<Store>
    {
       new Store? GetById(int id);
    }
}
