using Kwetter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kwetter.Repository.Patterns
{
    public class UnitOfWork : IUnitOfWorkAsync
    {
        private readonly DbContext _context;
        protected IDbContextTransaction Transaction;
        protected Dictionary<string, dynamic> Repositories;

        public int? CommandTimeout
        {
            get => _context.Database.GetCommandTimeout();
            set => _context.Database.SetCommandTimeout(value);
        }

        public UnitOfWork(DbContext context)
        {
            _context = context;
            Repositories = new Dictionary<string, dynamic>();
        }

        public virtual IRepository<TEntity> Repository<TEntity>() where TEntity : class, IModel
        {
            return RepositoryAsync<TEntity>();
        }


        public virtual int SaveChanges() => _context.SaveChanges();
        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken) => _context.SaveChangesAsync(cancellationToken);

        public virtual IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, IModel
        {
            if (Repositories == null)
            {
                Repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;

            if (Repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)Repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            Repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context, this));

            return Repositories[type];
        }

        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public virtual async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlCommandAsync(sql, parameters);
        }

        public virtual async Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlCommandAsync(sql, cancellationToken, parameters);
        }

        public virtual void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if (_context.Database.CurrentTransaction == null)
            {
                Transaction = _context.Database.BeginTransaction();
            }
            else
            {
                Transaction = _context.Database.CurrentTransaction;
            }

        }

        public virtual bool Commit()
        {
            Transaction.Commit();
            return true;
        }

        public virtual void Rollback()
        {
            Transaction.Rollback();
        }
    }
}
