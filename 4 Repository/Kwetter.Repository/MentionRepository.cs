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
    public interface IMentionRepository : IRepositoryAsync<Mention>
    {
    }

    public class MentionRepository : Repository<Mention>, IMentionRepository
    {
        public MentionRepository(DbContext context, IUnitOfWorkAsync unitOfWork) : base(context, unitOfWork)
        {
        }

        public override async Task<ICollection<Mention>> FindRangeAsync(Expression<Func<Mention, bool>> query)
        {
            return await base.Queryable()
                .Include(m => m.Kweet)
                .Where(query)
                .ToListAsync();
        }
    }
}
