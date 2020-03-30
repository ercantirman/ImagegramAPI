using System.Linq;

namespace ImagegramAPI.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ImagegramDbContext _context;

        public Repository(ImagegramDbContext context)
        {
            _context = context;
        }

        public T Get(long id)
        {
            return _context.Set<T>().Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
        public void Delete(long id)
        {
            var entity = Get(id);
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
    }
}
