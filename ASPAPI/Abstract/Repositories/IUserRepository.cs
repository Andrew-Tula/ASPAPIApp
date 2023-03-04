using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IUserRepository: IGenericRepository<User> {
        Role? GetRole(int id);
        User? GetByName(string name);
    }
}