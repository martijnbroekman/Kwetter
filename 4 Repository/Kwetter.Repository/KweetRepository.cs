using Kwetter.Models;
using Kwetter.Repository.Patterns;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Repository
{
    public interface IKweetRepository : IRepositoryAsync<Kweet>
    {
        Task<ICollection<Kweet>> GetNewestKweetsAsync(int from, int to);
        Task<ICollection<Kweet>> GetNewestKweetsByQueryAsync(Expression<Func<Kweet, bool>> query, int from, int to);
    }

    public class KweetRepository : Repository<Kweet>, IKweetRepository
    {
        public KweetRepository(DbContext context, IUnitOfWorkAsync unitOfWork) : base(context, unitOfWork)
        {
        }

        public override async Task<Kweet> FindAsync(Expression<Func<Kweet, bool>> query)
        {
            return await Queryable()
                .Include(k => k.Likes)
                .Include(k => k.User)
                .FirstOrDefaultAsync(query);
        }

        public override Kweet Find(Expression<Func<Kweet, bool>> query)
        {
            return Queryable()
                .Include(k => k.Likes)
                .Include(k => k.User)
                .FirstOrDefault(query);
        }

        public async Task<ICollection<Kweet>> GetNewestKweetsAsync(int from, int to)
        {
            return await Queryable()
                .OrderByDescending(K => K.Date)
                .Skip(from)
                .Take(to - from)
                .Include(k => k.Likes)
                .Include(k => k.User)
                .ToListAsync();
        }

        public async Task<ICollection<Kweet>> GetNewestKweetsByQueryAsync(Expression<Func<Kweet, bool>> query, int from, int to)
        {
            return await Queryable()
                .Include(k => k.Likes)
                .Include(k => k.User)
                .ThenInclude(u => u.Followers)
                .Where(query)
                .OrderByDescending(k => k.Date)
                .Skip(from)
                .Take(to - from)
                .ToListAsync();
        }
    }
}
