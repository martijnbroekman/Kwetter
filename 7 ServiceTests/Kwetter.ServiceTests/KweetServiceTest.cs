using Kwetter.Models;
using Kwetter.Repository;
using Kwetter.Repository.Patterns;
using Kwetter.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.ServiceTests
{
    [TestClass]
    public class KweetServiceTest : ServiceTestBase
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetKweetWithLikesAsyncShouldThowArgumentExceptionTest()
        {
            await _kweetService.GetKweetWithLikesAsync(0);
            Assert.Fail();
        }

        [TestMethod]
        public async Task GetKweetWithLikesShouldAsyncTest()
        {
            var kweet = await _kweetService.GetKweetWithLikesAsync(1);
            Assert.AreSame(kweet1, kweet);
            Assert.IsTrue(kweet1.Likes.Contains(like1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetAllNewestAsyncShouldThrowArgumentExceptionForToSmallToTest()
        {
            await _kweetService.GetAllNewestAsync(10, 9);
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetAllNewestAsyncShouldThrowArgumentExceptionForToLargeRangeTest()
        {
            await _kweetService.GetAllNewestAsync(0, 41);
            Assert.Fail();
        }

        [TestMethod]
        public async Task GetAllNewestAsyncTest()
        {
            var kweets = await _kweetService.GetAllNewestAsync(0, 1);
            Assert.AreEqual(1, kweets.Count);
            Assert.AreSame(kweets.ElementAt(0), kweet2);

            kweets = await _kweetService.GetAllNewestAsync(0, 2);
            Assert.AreEqual(2, kweets.Count);
            Assert.AreSame(kweets.ElementAt(0), kweet2);
            Assert.AreSame(kweets.ElementAt(1), kweet1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetTimeLineForUserIdAsyncThrowArgumentExceptionForToSmallToTest()
        {
            await _kweetService.GetTimeLineForUserIdAsync(1 ,10, 9);
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetTimeLineForUserIdAsyncThrowArgumentExceptionForIdTest()
        {
            await _kweetService.GetTimeLineForUserIdAsync(0, 0, 1);
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetTimeLineForUserIdAsynccShouldThrowArgumentExceptionForToLargeRangeTest()
        {
            await _kweetService.GetTimeLineForUserIdAsync(1, 0, 41);
            Assert.Fail();
        }

        [TestMethod]
        public async Task GetTimeLineForUserIdAsyncForOwnKweetsTest()
        {
            var kweets = await _kweetService.GetTimeLineForUserIdAsync(1, 0, 10);
            Assert.AreEqual(2, kweets.Count);   

        }

        [TestMethod]
        public async Task GetTimeLineForUserIdAsyncForOwnAndFollowingKweetsTest()
        {
            _followerService.Insert(
                new Follower
                {
                    Id = 1,
                    Date = new DateTime(2018, 3, 6),
                    User = user1,
                    UserId = user1.Id,
                    Follows = user2,
                    FollowsId = user2.Id
                });

            await _unitOfWork.SaveChangesAsync();

            var kweets = await _kweetService.GetTimeLineForUserIdAsync(1, 0, 10);
            Assert.AreEqual(5, kweets.Count);
        }

        [TestMethod]
        public async Task SearchKweetAsyncTest()
        {
            var kweets = await _kweetService.SearchKweetAsync("first", 0, 5);
            Assert.AreSame(kweet1, kweets.First());
            Assert.AreEqual(1, kweets.Count);

            kweets = await _kweetService.SearchKweetAsync("kweet", 0, 5);
            Assert.AreEqual(5, kweets.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task SearchKweetAsynccShouldThrowArgumentExceptionForToLargeRangeTest()
        {
            await _kweetService.SearchKweetAsync("first", 0, 41);
            Assert.Fail();
        }

        private async Task Seed()
        {
            user1 = new ApplicationUser
            {
                Id = 1,
                FirstName = "User1",
                MiddleName = "Middle",
                LastName = "Lastname"
            };
            _userService.Insert(user1);

            user2 = new ApplicationUser
            {
                Id = 2,
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
    }
}
