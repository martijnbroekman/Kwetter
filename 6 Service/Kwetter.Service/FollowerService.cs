using Kwetter.Models;
using Kwetter.Repository;
using Kwetter.Repository.Patterns;
using Kwetter.Service.Helpers;
using Kwetter.Service.Patterns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Service
{
    public interface IFollowerService : IService<Follower>
    {
        bool TryFindByUserAndFollowsId(int userId, int followsId, out Follower follower);
        Task RemoveRangeByUserIdAsync(int userId);
    }

    public class FollowerService : Service<Follower>, IFollowerService
    {
        private readonly IRepositoryAsync<Follower> _repository;

        public FollowerService(IRepositoryAsync<Follower> repository) : base(repository)
        {
            _repository = repository;
        }

        public bool TryFindByUserAndFollowsId(int userId, int followsId, out Follower follower)
        {
            ServiceHelpers.CheckValidId(userId);
            ServiceHelpers.CheckValidId(followsId);

            follower = _repository.Find(f => f.UserId.Equals(userId) && f.FollowsId.Equals(followsId));
            return follower != null;
        }

        public async Task RemoveRangeByUserIdAsync(int userId)
        {
            ServiceHelpers.CheckValidId(userId);

            var followers = await _repository.FindRangeAsync(f => f.FollowsId.Equals(userId) || f.UserId.Equals(userId));
            if (followers.Count > 0)
                _repository.DeleteRange(followers);
        }
    }
}
