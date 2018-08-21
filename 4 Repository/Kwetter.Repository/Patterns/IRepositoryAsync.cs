using Kwetter.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Repository.Patterns
{
    public interface IRepositoryAsync<TEntity> : IRepository<TEntity> where TEntity : class, IModel
    {
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> query);
        Task<ICollection<TEntity>> FindRangeAsync(Expression<Func<TEntity, bool>> query);

        Task InsertAsync(TEntity entity);

        Task InsertRangeAsync(IEnumerable<TEntity> entities);
    }
}
