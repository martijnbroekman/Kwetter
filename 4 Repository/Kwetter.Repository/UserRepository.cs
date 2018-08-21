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

    public interface IUserRepository : IRepositoryAsync<ApplicationUser>
    {
        Task<ICollection<ApplicationUser>> GetRange(int from, int to);
        Task<ICollection<ApplicationUser>> GetFollowing(int userId);
        Task<ICollection<ApplicationUser>> GetFollowers(int userId);
        Task<ICollection<ApplicationUser>> GetAllWithRoles();
    }

    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(DbContext context, IUnitOfWorkAsync unitOfWork) : base(context, unitOfWork)
        {
        }

        public override Task<ApplicationUser> FindAsync(Expression<Func<ApplicationUser, bool>> query)
        {
            return base.Queryable().Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefaultAsync(query);

        }

        public async Task<ICollection<ApplicationUser>> GetRange(int from, int to)
        {
            return await Queryable()
                .Skip(from)
                .Take(to - from)
                .ToListAsync();
        }

        public async Task<ICollection<ApplicationUser>> GetAllWithRoles()
        {
            return await Queryable()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<ICollection<ApplicationUser>> GetFollowing(int userId)
        {
            var followerQueryable = GetRepository<Follower>().Queryable();

            return await followerQueryable
                .Where(f => f.UserId.Equals(userId))
                .Include(f => f.Follows)
                .Select(f => f.Follows)
                .ToListAsync();
        }

        public async Task<ICollection<ApplicationUser>> GetFollowers(int userId)
        {
            var followerQueryable = GetRepository<Follower>().Queryable();

            return await followerQueryable
                .Where(f => f.FollowsId.Equals(userId))
                .Include(f => f.User)
                .Select(f => f.User)
                .ToListAsync();
        }
    }
}
