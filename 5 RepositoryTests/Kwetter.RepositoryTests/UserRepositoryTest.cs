using Kwetter.Models;
using Kwetter.Repository;
using Kwetter.Repository.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.RepositoryTests
{
    [TestClass]
    public class UserRepositoryTest : RepositoryTestBase
    {
        private IUnitOfWorkAsync _unitOfWork;

        private IKweetRepository _kweetRepository;
        private IUserRepository _userRepository;
        private IRepositoryAsync<Like> _likeRepository;
        private IRepositoryAsync<Follower> _followerRepository;

        private ApplicationUser user1;
        private ApplicationUser user2;

        private Kweet kweet1;
        private Kweet kweet2;

        private Like like1;

        [TestInitialize]
        public async Task Init()
        {
            _unitOfWork = new UnitOfWork(_context);
            _kweetRepository = new KweetRepository(_context, _unitOfWork);
            _userRepository = new UserRepository(_context, _unitOfWork);
            _likeRepository = new Repository<Like>(_context, _unitOfWork);
            _followerRepository = new Repository<Follower>(_context, _unitOfWork);

            await Seed();
        }
        
        [TestMethod]
        public async Task GetRangeWithToSmallTest()
        {
            await InsertUsers(7);
            var users = await _userRepository.GetRange(0, 10);
            Assert.AreEqual(9, users.Count);
        }

        [TestMethod]
        public async Task GetRangeWithMoreUsersTest()
        {
            await InsertUsers(9);
            var users = await _userRepository.GetRange(0, 10);
            Assert.AreEqual(10, users.Count);
        }
        
        [TestMethod]
        public async Task GetFollowingTest()
        {
            var following = await _userRepository.GetFollowing(user1.Id);
            Assert.AreEqual(0, following.Count);

            _followerRepository.Insert(new Follower
            {
                User = user1,
                UserId = user1.Id,
                Follows = user2,
                FollowsId = user2.Id
            });
            await _unitOfWork.SaveChangesAsync();

            following = await _userRepository.GetFollowing(user1.Id);
            Assert.IsTrue(following.Contains(user2));
        }
        
        [TestMethod]
        public async Task GetFollowersTest()
        {
            var followers = await _userRepository.GetFollowers(user1.Id);
            Assert.AreEqual(0, followers.Count);

            _followerRepository.Insert(new Follower
            {
                User = user2,
                UserId = user2.Id,
                Follows = user1,
                FollowsId = user1.Id
            });
            await _unitOfWork.SaveChangesAsync();

            followers = await _userRepository.GetFollowers(user1.Id);
            Assert.IsTrue(followers.Contains(user2));
        }

        private async Task Seed()
        {
            user1 = new ApplicationUser
            {
                FirstName = "User1",
                MiddleName = "Middle",
                LastName = "Lastname"
            };
            _userRepository.Insert(user1);

            user2 = new ApplicationUser
            {
                FirstName = "User2",
                MiddleName = "Middle",
                LastName = "Lastname"
            };
            _userRepository.Insert(user2);
            await _unitOfWork.SaveChangesAsync();

            kweet1 = new Kweet
            {
                Id = 1,
                User = user1,
                UserId = user1.Id,
                Description = "first kweet",
                Date = new DateTime(2018, 3, 7)
            };
            _kweetRepository.Insert(kweet1);

            kweet2 = new Kweet
            {
                Id = 2,
                User = user1,
                UserId = user1.Id,
                Description = "second kweet",
                Date = new DateTime(2018, 3, 8)
            };
            _kweetRepository.Insert(kweet2);

            _kweetRepository.Insert(
                new Kweet
                {
                    Id = 3,
                    User = user2,
                    UserId = user2.Id,
                    Description = "third kweet",
                    Date = new DateTime(2018, 3, 6)
                });

            _kweetRepository.Insert(
                new Kweet
                {
                    Id = 4,
                    User = user2,
                    UserId = user2.Id,
                    Description = "fourth kweet",
                    Date = new DateTime(2018, 3, 6)
                });

            _kweetRepository.Insert(
                new Kweet
                {
                    Id = 5,
                    User = user2,
                    UserId = user2.Id,
                    Description = "fifth kweet",
                    Date = new DateTime(2018, 3, 6)
                });

            await _unitOfWork.SaveChangesAsync();

            like1 = new Like
            {
                Id = 1,
                Kweet = kweet1,
                KweetId = kweet1.Id,
                User = user1,
                UserId = user1.Id
            };
            _likeRepository.Insert(like1);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task InsertUsers(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _userRepository.Insert(new ApplicationUser
                {
                    UserName = $"Username{i}",
                    Email = $"Email{i}",
                    FirstName = $"Firstname{i}",
                });
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
