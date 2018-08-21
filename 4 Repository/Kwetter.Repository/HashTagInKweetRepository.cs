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
    public interface IHashTagInKweetRepository : IRepositoryAsync<HashTagInKweet>
    {
    }

    public class HashTagInKweetRepository : Repository<HashTagInKweet>, IHashTagInKweetRepository
    {
        public HashTagInKweetRepository(DbContext context, IUnitOfWorkAsync unitOfWork) : base(context, unitOfWork)
        {
        }

        public override async Task<ICollection<HashTagInKweet>> FindRangeAsync(Expression<Func<HashTagInKweet, bool>> query)
        {
            return await base.Queryable()
                .Include(h => h.Kweet)
                .Where(query)
                .ToListAsync();
        }
    }
}
