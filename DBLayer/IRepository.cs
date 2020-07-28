using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBLayer
{
    public interface IRepository<TEntity> where TEntity : class
    {

        #region preDefined
        TEntity Find(params object[] keyValues);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<TEntity> FindAsync(params object[] keyValues);
        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        #endregion

        #region Extensions
        bool Any(Expression<Func<TEntity, bool>> predicate);
       
        IQueryable<TEntity> AsQueryable();
        IQueryable<TEntity> AsNoTracking();

        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllEager(List<string> paths);
        IQueryable<TEntity> FindEager(Expression<Func<TEntity, bool>> predicate, List<string> paths);
        #endregion
    }
}
