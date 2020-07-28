using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBLayer.Repos
{

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly InventoryContext _context;
        public Repository(InventoryContextScope DbContextScope)
        {
            if (DbContextScope == null)
            {
                throw new ArgumentNullException("DbContextScope");
            }

            _context = DbContextScope.Create();
        }

        public IQueryable<TEntity> masterEntities => _context.Set<TEntity>().AsQueryable();

        public TEntity Add(TEntity entity)
        {
            return _context.Set<TEntity>().Add(entity);
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            return _context.Set<TEntity>().AddRange(entities);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Any<TEntity>(predicate);
        }

        public IQueryable<TEntity> AsNoTracking()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public TEntity Find(params object[] keyValues)
        {
            return _context.Set<TEntity>().Find(keyValues);
        }

        public Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return _context.Set<TEntity>().FindAsync(cancellationToken,keyValues);
        }

        public Task<TEntity> FindAsync(params object[] keyValues)
        {
            return _context.Set<TEntity>().FindAsync( keyValues);
        }

        public IQueryable<TEntity> FindEager(Expression<Func<TEntity, bool>> predicate, List<string> paths)
        {
            var query = _context.Set<TEntity>().Where(predicate);

            foreach (string path in paths)
            {
                query = query.Include(path);
            }

            return query;
                }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> GetAllEager(List<string> paths)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            foreach (string path in paths)
            {
                query = query.Include(path);
            }

            return query;
        }

    }
}
