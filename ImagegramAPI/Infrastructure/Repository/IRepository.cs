using System.Linq;

namespace ImagegramAPI.Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        T Get(long id);

        IQueryable<T> GetAll();

        void Create(T entity);

        void Update(T entity);

        void Delete(long id);
    }
}
