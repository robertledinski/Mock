using DatabaseAbstructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInfrastructure.Abstractions
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;
        public Repository(ApplicationContext context)
        {
            _context = context;
        }

        public void Add(T entity)
            => _context.Set<T>().Add(entity);
        
        public void AddRange(IEnumerable<T> entities)
            => _context.Set<T>().AddRange(entities);
        
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
            => _context.Set<T>().Where(expression);
        
        public IEnumerable<T> GetAll()
            => _context.Set<T>().ToList();
        
        public T GetById(int id)
            => _context.Set<T>().Find(id);
        
        public void Remove(T entity)
            => _context.Set<T>().Remove(entity);

        public void RemoveRange(IEnumerable<T> entities)
            => _context.Set<T>().RemoveRange(entities);

        public IQueryable<T> All()
            => _context.Set<T>().AsQueryable();
    }
}
