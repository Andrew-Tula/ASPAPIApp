using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IUserRepository: IGenericRepositories<User> {
        Role? GetRole(int id);
    }
}