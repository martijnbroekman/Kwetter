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
    public interface IHashTagRepository : IRepositoryAsync<HashTag>
    {
        Task<IEnumerable<HashTag>> GetTopAsync(int amount);
    }

    public class HashTagRepository : Repository<HashTag>, IHashTagRepository
    {
        public HashTagRepository(DbContext context, IUnitOfWorkAsync unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<IEnumerable<HashTag>> GetTopAsync(int amount)
        {
            var hashTagInKweetQueryable = GetRepository<HashTagInKweet>().Queryable();

            return await hashTagInKweetQueryable
                .Include(hk => hk.Kweet)
                .Include(hk => hk.HashTag)
                .GroupBy(hk => hk.HashTag)
                .OrderByDescending(g => g.Count(hk => hk.Kweet.Date >= DateTime.Now.AddDays(-7)))
                .Select(g => g.Key)
                .Take(amount)
                .ToListAsync();
        }
    }
}
