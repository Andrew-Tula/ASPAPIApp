using ASPAPI.Models.DbEntities;

namespace ASPAPI.Abstract.Repositories
{
    public interface IUserTokeRepository: IGenericRepository<UserToken>
    {
        UserToken? GetByUser(int userId);
        UserToken? FindByToken(string token);
    }
}