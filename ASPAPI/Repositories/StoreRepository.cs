using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Repositories
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository 
    {
        public StoreRepository(TestDBContext dbContext) : base(dbContext) { }

        public new Store? GetById(int id) => dbContext.Stores.FirstOrDefault(s => s.Id == id);
    }
}
