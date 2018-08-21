using Kwetter.Models;
using Kwetter.Repository.Patterns;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetter.Service.Patterns
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class, IModel
    {
        private IRepositoryAsync<TEntity> _repository;

        public Service(IRepositoryAsync<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual TEntity Find(int id)
        {
            return _repository.Find(e => e.Id.Equals(id));
        }

        public bool TryFind(int id, out TEntity entity)
        {
            entity = _repository.Find(e => e.Id.Equals(id));
            return entity != null;
        }

        public virtual TEntity Insert(TEntity entity)
        {
            _repository.Insert(entity);
            return entity;
        }

        public virtual bool Remove(int id)
        {
            _repository.Delete(id);
            return true;
        }

        public virtual TEntity Update(TEntity entity)
        {
            _repository.Update(entity);
            return entity;
        }
    }
}
