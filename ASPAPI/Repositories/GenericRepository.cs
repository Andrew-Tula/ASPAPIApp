using ASPAPI.Abstract.Models;
using ASPAPI.Abstract.Repositories;
using ASPAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace ASPAPI.Repositories
{
    public class GenericRepository<T> : IGenericRepositories<T> where T : class, IEntity {
        protected TestDBContext dbContext;
        protected DbSet<T> dbSet;

        public GenericRepository(TestDBContext dbContext) {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }
        public List<T>? GetAll() => dbSet?.ToList();

        public virtual T? GetById(int id) => dbSet.FirstOrDefault(d => d.Id == id);
        public virtual T? GetProduct(int id) => dbSet.FirstOrDefault(p => p.Id == id);
        public virtual T? GetOrder(int id) => dbSet.FirstOrDefault(o => o.Id == id);

        public void Add(T item) {
            dbSet.Add(item);
            dbContext.SaveChanges();
        }

        public void Update(T item) {
            dbContext.Update(item);
            dbContext.SaveChanges();
        }

        public virtual void Remove(T item) {
            dbContext.Remove(item);
            dbContext.SaveChanges();
        }

        public void InitDbSet(DbSet<T> dbSet) {
            this.dbSet = dbSet;
        }
    }
}
