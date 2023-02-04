using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(TestDBContext dbContext) : base(dbContext) { }

        public User? GetUser(int id) => dbContext.Users.FirstOrDefault(u => u.Id == id);
    }
}
