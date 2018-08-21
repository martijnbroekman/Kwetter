using Kwetter.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetter.Service.Patterns
{
    public interface IService<TEntity> where TEntity : class, IModel
    {
        TEntity Find(int id);
        bool TryFind(int id, out TEntity entity);
        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        bool Remove(int id);
    }
}