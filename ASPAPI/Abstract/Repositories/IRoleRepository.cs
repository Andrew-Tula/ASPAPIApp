using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IRoleRepository: IGenericRepositories<Role>
    {
        Role? GetByName(string name);
    }
}