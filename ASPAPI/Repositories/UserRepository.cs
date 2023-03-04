using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository {
        public UserRepository(TestDBContext dbContext) : base(dbContext) { }

        public User? GetByName(string name) => dbSet.FirstOrDefault(d => d.Name == name);

        public Role? GetRole(int id) => dbContext.Roles.FirstOrDefault(r => r.Id == id);
    }
}
