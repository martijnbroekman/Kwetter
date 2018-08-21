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
    public interface IHashTagInKweetService : IService<HashTagInKweet>
    {
        Task InsertRangeForKweet(Kweet kweet, ICollection<HashTag> hashTags);
        Task RemoveRangeByUserIdAsync(int userId);
        Task RemoveRangeByKweetIdAsync(int kweetId);
    }

    public class HashTagInKweetService : Service<HashTagInKweet>, IHashTagInKweetService
    {
        private readonly IHashTagInKweetRepository _repository;

        public HashTagInKweetService(IHashTagInKweetRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task InsertRangeForKweet(Kweet kweet, ICollection<HashTag> hashTags)
        {
            foreach (var hashTag in hashTags)
            {
                await _repository.InsertAsync(new HashTagInKweet
                {
                    HashTag = hashTag,
                    HashTagId = hashTag.Id,
                    Kweet = kweet,
                    KweetId = kweet.Id
                });
            }
        }

        public async Task RemoveRangeByUserIdAsync(int userId)
        {
            ServiceHelpers.CheckValidId(userId);

            var hashTagInKweets = await _repository.FindRangeAsync(h => h.Kweet.UserId.Equals(userId));
            if (hashTagInKweets.Count > 0)
                _repository.DeleteRange(hashTagInKweets);
        }
        
        public async Task RemoveRangeByKweetIdAsync(int kweetId)
        {
            ServiceHelpers.CheckValidId(kweetId);

            var hashTagInKweets = await _repository.FindRangeAsync(h => h.KweetId.Equals(kweetId));
            if (hashTagInKweets.Count > 0)
                _repository.DeleteRange(hashTagInKweets);
        }
    }
}
