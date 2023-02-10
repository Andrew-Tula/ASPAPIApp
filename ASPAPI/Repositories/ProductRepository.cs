using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace ASPAPI.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository {
        public ProductRepository(TestDBContext dbContext) : base(dbContext) { }

        public override Product? GetById(int id)
        {
            return dbSet.Where(d => d.Id == id).Include(c => c.OrderItems)?.FirstOrDefault();
        }

        public override void Remove(Product item)
        {
            var orderitems = dbContext.OrderItems.Where(o => o.ProductId == item.Id)?.ToList();
            if (orderitems?.Count > 0)
                return;

            base.Remove(item);
        }
    }
}
