using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IRoleRepository: IGenericRepository<Role>
    {
        Role? GetByName(string name);
    }
}