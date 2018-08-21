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
    public interface ILikeService : IService<Like> 
    {
        bool IsKweetLiked(int userId, int kweetId);
        bool TryGetLikeByUserAndKweetId(int userId, int kweetId, out Like like);
        Task RemoveRangeByUserIdAsync(int userId);
        Task RemoveRangeByKweetIdAsync(int kweetId);
    }

    public class LikeService : Service<Like>, ILikeService
    {
        private readonly IRepositoryAsync<Like> _repository;

        public LikeService(IRepositoryAsync<Like> repository) : base(repository)
        {
            _repository = repository;
        }

        public bool IsKweetLiked(int userId, int kweetId)
        {
            return GetLikeByUserAndKweetId(userId, kweetId) != null;
        }

        public async Task RemoveRangeByUserIdAsync(int userId)
        {
            ServiceHelpers.CheckValidId(userId);

            var likes = await _repository.FindRangeAsync(l => l.UserId.Equals(userId));
            if (likes.Count > 0)
                _repository.DeleteRange(likes);
        }
        
        public async Task RemoveRangeByKweetIdAsync(int kweetId)
        {
            ServiceHelpers.CheckValidId(kweetId);

            var likes = await _repository.FindRangeAsync(l => l.KweetId.Equals(kweetId));
            if (likes.Count > 0)
                _repository.DeleteRange(likes);
        }

        public bool TryGetLikeByUserAndKweetId(int userId, int kweetId, out Like like)
        {
            like = GetLikeByUserAndKweetId(userId, kweetId);
            return like != null;
        }

        private Like GetLikeByUserAndKweetId(int userId, int kweetId)
        {
            ServiceHelpers.CheckValidId(userId);
            ServiceHelpers.CheckValidId(kweetId);

            return _repository.Find(l => l.UserId.Equals(userId) && l.KweetId.Equals(kweetId));
        }
    }
}
