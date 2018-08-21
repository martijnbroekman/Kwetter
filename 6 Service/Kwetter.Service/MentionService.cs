using Kwetter.Models;
using Kwetter.Repository;
using Kwetter.Repository.Patterns;
using Kwetter.Service.Patterns;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Service
{
    public interface IMentionService : IService<Mention>
    {
        Task RemoveRangeByUserIdAsync(int userId);
        Task RemoveRangeByKweetIdAsync(int kweetId);
    }

    public class MentionService : Service<Mention>, IMentionService
    {
        private readonly IMentionRepository _repository;

        public MentionService(IMentionRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task RemoveRangeByUserIdAsync(int userId)
        {
            var mentions = await _repository.FindRangeAsync(m => m.Kweet.UserId.Equals(userId) || m.UserId.Equals(userId));
            if (mentions.Count > 0)
                _repository.DeleteRange(mentions);
        }
        
        public async Task RemoveRangeByKweetIdAsync(int kweetId)
        {
            var mentions = await _repository.FindRangeAsync(m => m.KweetId.Equals(kweetId));
            if (mentions.Count > 0)
                _repository.DeleteRange(mentions);
        }
    }
}
