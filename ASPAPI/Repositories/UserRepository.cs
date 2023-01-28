using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Repositories
{
    public class UserRepository : GenericRepositories<User>, IUserRepository {
        public UserRepository(TestDBContext dbContext) : base(dbContext) { }

        public Role? GetRole(int id) => dbContext.Roles.FirstOrDefault(r => r.Id == id);
    }
}
