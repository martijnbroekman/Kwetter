using Kwetter.Models;
using Kwetter.Repository;
using Kwetter.Repository.Patterns;
using Kwetter.Service.Helpers;
using Kwetter.Service.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kwetter.Service
{
    public interface IHashTagService : IService<HashTag>
    {
        Task<IEnumerable<HashTag>> GetTopAsync(int amount);
        ICollection<HashTag> GetHashTagsFromKweetDescription(string description);
        Task<ICollection<HashTag>> InsertRangeAsync(ICollection<HashTag> hashTags);
    }

    public class HashTagService : Service<HashTag>, IHashTagService
    {
        private readonly IHashTagRepository _repository;

        public HashTagService(IHashTagRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<HashTag>> GetTopAsync(int amount)
        {
            ServiceHelpers.CheckValidId(amount);

            return await _repository.GetTopAsync(amount);
        }

        public ICollection<HashTag> GetHashTagsFromKweetDescription(string description)
        {
            var pattern = @"\B#\w\w+";
            var matches = Regex.Matches(description, pattern);
            return matches.Cast<Match>().Select(match => match.Value).Distinct().Select(v => new HashTag { Title = v }).ToList();
        }

        public override HashTag Insert(HashTag hashTag)
        {
            var foundHashTag = _repository.Find(h => h.Title.Equals(hashTag.Title));
            if (foundHashTag == null)
                return base.Insert(hashTag);
            else
                return foundHashTag;
        }
        
        public async Task<ICollection<HashTag>> InsertRangeAsync(ICollection<HashTag> hashTags)
        {
            var insertedHashtags = new List<HashTag>();
            foreach (var hashTag in hashTags)
            {
                var foundHashTag = await _repository.FindAsync(h => h.Title.Equals(hashTag.Title));
                if (foundHashTag == null)
                {
                    await _repository.InsertAsync(hashTag);
                    insertedHashtags.Add(hashTag);
                }
                else
                {
                    insertedHashtags.Add(foundHashTag);
                }
            }
            return insertedHashtags;            
        }

    }
}
