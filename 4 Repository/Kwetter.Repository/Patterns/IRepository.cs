using Kwetter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kwetter.Repository.Patterns
{
    public interface IRepository<TEntity> where TEntity : class, IModel
    {
        TEntity Find(Expression<Func<TEntity, bool>> query);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(params object[] keyValues);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Queryable();
        IRepository<T> GetRepository<T>() where T : class, IModel;
    }
}
