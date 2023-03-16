using ASPAPI.Abstract.Models;

namespace ASPAPI.Abstract.Repositories
{
    public interface IGenericRepositories<T> where T : class, IEntity
    {
        void Add(T item);
        T? GetById(int id);
        List<T>? GetAll();
        void Remove(T item);
        void Update(T item);
        //  сомневаюсь - сюда ли добавлять ?
        T? GetProduct(int id);    
        T? GetOrder(int id);
       
    }
}