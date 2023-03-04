using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Repositories
{
    public class UserTokeRepository : GenericRepository<UserToken>, IUserTokeRepository {
        public UserTokeRepository(TestDBContext dbContext) : base(dbContext) { }

        public UserToken? FindByToken(string refreshToken) => dbSet.FirstOrDefault(x => x.RefreshToken == refreshToken);

        public UserToken? GetByUser(int userId) => dbSet.FirstOrDefault(x => x.UserId == userId);
    }
}
