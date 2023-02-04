using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository {
        public RoleRepository(TestDBContext dbContext) : base(dbContext) { }

        public Role? GetByName(string name) => dbSet.FirstOrDefault(r => r.Name == name);
    }
}
