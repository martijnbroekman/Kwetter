using Kwetter.Models;
using Kwetter.Repository;
using Kwetter.Repository.Patterns;
using Kwetter.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.ServiceTests
{
    [TestClass]
    public class UserServiceTest : ServiceTestBase
    {
        private IUnitOfWorkAsync _unitOfWork;

        private IKweetService _kweetService;
        private IUserService _userService;
        private ILikeService _likeService;
        private IFollowerService _followerService;

        private ApplicationUser user1;
        private ApplicationUser user2;

        private Kweet kweet1;
        private Kweet kweet2;

        private Like like1;

        [TestInitialize]
        public async Task Init()
        {

            _unitOfWork = new UnitOfWork(_context);
            _kweetService = new KweetService(new KweetRepository(_context, _unitOfWork));
            _userService = new UserService(new UserRepository(_context, _unitOfWork));
            _likeService = new LikeService(new Repository<Like>(_context, _unitOfWork));
            _followerService = new FollowerService(new Repository<Follower>(_context, _unitOfWork));

            await Seed();
        }

        #region GetRange tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetRangeShouldThrowArgumentExceptionForToSmallToTest()
        {
            await _userService.GetRange(10, 9);
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetRangeShouldThrowArgumentExceptionForToLargeRangeTest()
        {
            await _userService.GetRange(0, 41);
            Assert.Fail();
        }

        [TestMethod]
        public async Task GetRangeWithToSmallTest()
        {
            await InsertUsers(7);
            var users = await _userService.GetRange(0, 10);
            Assert.AreEqual(9, users.Count);
        }

        [TestMethod]
        public async Task GetRangeWithMoreUsersTest()
        {
            await InsertUsers(9);
            var users = await _userService.GetRange(0, 10);
            Assert.AreEqual(10, users.Count);
        }
        #endregion

        #region GetFollowing tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFollowingShouldThrowArgumentExceptionTest()
        {
            await _userService.GetFollowing(0);
            Assert.Fail();
        }

        [TestMethod]
        public async Task GetFollowingTest()
        {
            var following = await _userService.GetFollowing(user1.Id);
            Assert.AreEqual(0, following.Count);

            _followerService.Insert(new Follower
            {
                User = user1,
                UserId = user1.Id,
                Follows = user2,
                FollowsId = user2.Id
            });
            await _unitOfWork.SaveChangesAsync();

            following = await _userService.GetFollowing(user1.Id);
            Assert.IsTrue(following.Contains(user2));
        }
        #endregion

        #region GetFollowers tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetFollowersShouldThrowArgumentExceptionTest()
        {
            await _userService.GetFollowers(0);
            Assert.Fail();
        }

        [TestMethod]
        public async Task GetFollowersTest()
        {
            var followers = await _userService.GetFollowers(user1.Id);
            Assert.AreEqual(0, followers.Count);

            _followerService.Insert(new Follower
            {
                User = user2,
                UserId = user2.Id,
                Follows = user1,
                FollowsId = user1.Id
            });
            await _unitOfWork.SaveChangesAsync();

            followers = await _userService.GetFollowers(user1.Id);
            Assert.IsTrue(followers.Contains(user2));
        }
        #endregion

        private async Task Seed()
        {
            user1 = new ApplicationUser
            {
                FirstName = "User1",
                MiddleName = "Middle",
                LastName = "Lastname"
            };
            _userService.Insert(user1);

            user2 = new ApplicationUser
            {
                FirstName = "User2",
                MiddleName = "Middle",
                LastName = "Lastname"
            };
            _userService.Insert(user2);
            await _unitOfWork.SaveChangesAsync();

            kweet1 = new Kweet
            {
                Id = 1,
                User = user1,
                UserId = user1.Id,
                Description = "first kweet",
                Date = new DateTime(2018, 3, 7)
            };
            _kweetService.Insert(kweet1);

            kweet2 = new Kweet
            {
                Id = 2,
                User = user1,
                UserId = user1.Id,
                Description = "second kweet",
                Date = new DateTime(2018, 3, 8)
            };
            _kweetService.Insert(kweet2);

            _kweetService.Insert(
                new Kweet
                {
                    Id = 3,
                    User = user2,
                    UserId = user2.Id,
                    Description = "third kweet",
                    Date = new DateTime(2018, 3, 6)
                });

            _kweetService.Insert(
                new Kweet
                {
                    Id = 4,
                    User = user2,
                    UserId = user2.Id,
                    Description = "fourth kweet",
                    Date = new DateTime(2018, 3, 6)
                });

            _kweetService.Insert(
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
            _likeService.Insert(like1);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task InsertUsers(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _userService.Insert(new ApplicationUser
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
