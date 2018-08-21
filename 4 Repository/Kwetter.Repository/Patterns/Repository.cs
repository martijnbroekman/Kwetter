using Kwetter.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Repository.Patterns
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IModel
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public Repository(DbContext context, IUnitOfWorkAsync unitOfWork)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _unitOfWork = unitOfWork;
        }

        public virtual void Delete(params object[] keyValues)
        {
            var entity = _dbSet.Find(keyValues);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity) => _context.Remove(entity);
        public virtual void DeleteRange(IEnumerable<TEntity> entities) => _context.RemoveRange(entities);

        public virtual TEntity Find(Expression<Func<TEntity, bool>> query) => _dbSet.FirstOrDefault(query);
        public virtual Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> query) => _dbSet.FirstOrDefaultAsync(query);

        public virtual void Insert(TEntity entity) => _context.Add(entity);
        public virtual Task InsertAsync(TEntity entity) => _context.AddAsync(entity);

        public virtual void InsertRange(IEnumerable<TEntity> entities) => _context.AddRange(entities);
        public virtual Task InsertRangeAsync(IEnumerable<TEntity> entities) => _context.AddRangeAsync(entities);

        public virtual IQueryable<TEntity> Queryable() => _dbSet;

        public virtual void Update(TEntity entity) => _context.Update(entity);

        public IRepository<T> GetRepository<T>() where T : class, IModel => _unitOfWork.Repository<T>();

        public virtual async Task<ICollection<TEntity>> FindRangeAsync(Expression<Func<TEntity, bool>> query) => await _dbSet.Where(query).ToListAsync();
    }
}
