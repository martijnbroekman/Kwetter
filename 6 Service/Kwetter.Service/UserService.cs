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
    public interface IUserService : IService<ApplicationUser>
    {
        Task<ICollection<ApplicationUser>> GetRange(int from, int to);
        Task<ICollection<ApplicationUser>> GetFollowing(int userId);
        Task<ICollection<ApplicationUser>> GetFollowers(int userId);
        Task<ICollection<ApplicationUser>> GetAllWithRoles();
        Task<ApplicationUser> GetById(int userId);
    }

    public class UserService : Service<ApplicationUser>, IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<ApplicationUser> GetById(int userId)
        {
            ServiceHelpers.CheckValidId(userId);

            return await _repository.FindAsync(u => u.Id.Equals(userId));
        }

        public async Task<ICollection<ApplicationUser>> GetRange(int from, int to)
        {
            ServiceHelpers.CheckRange(from, to);

            return await _repository.GetRange(from, to);
        }

        public async Task<ICollection<ApplicationUser>> GetFollowing(int userId)
        {
            ServiceHelpers.CheckValidId(userId);
            
            return await _repository.GetFollowing(userId);
        }

        public async Task<ICollection<ApplicationUser>> GetFollowers(int userId)
        {
            ServiceHelpers.CheckValidId(userId);

            return await _repository.GetFollowers(userId);
        }

        public async Task<ICollection<ApplicationUser>> GetAllWithRoles()
        {
            return await _repository.GetAllWithRoles();
        }
    }
}
