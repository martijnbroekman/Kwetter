using Kwetter.Models;
using Kwetter.Repository;
using Kwetter.Repository.Patterns;
using Kwetter.Service.Helpers;
using Kwetter.Service.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.Service
{
    public interface IKweetService : IService<Kweet>
    {
        Task<Kweet> GetKweetWithLikesAsync(int id);
        Task<ICollection<Kweet>> GetAllNewestAsync(int from, int to);
        Task<ICollection<Kweet>> GetAllNewestByUserIdAsync(int userId, int from, int to);
        Task<ICollection<Kweet>> GetTimeLineForUserIdAsync(int userId, int from, int to);
        Task<ICollection<Kweet>> SearchKweetAsync(string find, int from, int to);
    }

    public class KweetService : Service<Kweet>, IKweetService
    {
        private readonly IKweetRepository _repository;

        public KweetService(IKweetRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<Kweet> GetKweetWithLikesAsync(int id)
        {
            ServiceHelpers.CheckValidId(id);

            return await _repository.FindAsync(k => k.Id.Equals(id));
        }

        public async Task<ICollection<Kweet>> GetAllNewestAsync(int from, int to)
        {
            ServiceHelpers.CheckRange(from, to);

            return await _repository.GetNewestKweetsAsync(from, to);
        }

        public async Task<ICollection<Kweet>> GetAllNewestByUserIdAsync(int userId, int from, int to)
        {
            ServiceHelpers.CheckRange(from, to);

            return await _repository.GetNewestKweetsByQueryAsync(
                kweet => kweet.UserId.Equals(userId),
                from, 
                to);
        }
        
        public async Task<ICollection<Kweet>> GetTimeLineForUserIdAsync(int userId, int from, int to)
        {
            ServiceHelpers.CheckValidId(userId);
            ServiceHelpers.CheckRange(from, to);

            return await _repository.GetNewestKweetsByQueryAsync(
                (kweet => kweet.UserId.Equals(userId) || kweet.User.Followers.Any(f => f.UserId.Equals(userId))), 
                from, 
                to);
        }

        public async Task<ICollection<Kweet>> SearchKweetAsync(string find, int from, int to)
        {
            ServiceHelpers.CheckRange(from, to);

            return await _repository.GetNewestKweetsByQueryAsync(
                (kweet => kweet.Description.Contains(find)),
                from, 
                to);
        }

        
    }
}
