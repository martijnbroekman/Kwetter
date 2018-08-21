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
    public class LikeServiceTest : ServiceTestBase
    {
        private IUnitOfWorkAsync _unitOfWork;

        private IKweetService _kweetService;
        private IUserService _userService;
        private ILikeService _likeService;

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

            await Seed();
        }

        #region IsKweetLiked tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsKweetLikedShouldThrowArgumentExceptionForUserIdTest()
        {
            _likeService.IsKweetLiked(0, 1);
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsKweetLikedShouldThrowArgumentExceptionForKweetIdTest()
        {
            _likeService.IsKweetLiked(1, 0);
            Assert.Fail();
        }

        [TestMethod]
        public void IsKweetLikedTrueFlowTest()
        {
            var value = _likeService.IsKweetLiked(user1.Id, kweet1.Id);

            Assert.IsTrue(value);
        }

        [TestMethod]
        public void IsKweetLikedFalseFlowTest()
        {
            var value = _likeService.IsKweetLiked(user1.Id, kweet2.Id);

            Assert.IsFalse(value);
        }
        #endregion

        #region TryGetLikeByUserAndKweetId tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryGetLikeByUserAndKweetIdShouldThrowArgumentExceptionForUserIdTest()
        {
            _likeService.TryGetLikeByUserAndKweetId(0, 1, out var like);
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryGetLikeByUserAndKweetIdShouldThrowArgumentExceptionForKweetIdTest()
        {
            _likeService.TryGetLikeByUserAndKweetId(1, 0, out var like);
            Assert.Fail();
        }

        [TestMethod]
        public void TryGetLikeByUserAndKweetIdTrueFlowTest()
        {
            var value = _likeService.TryGetLikeByUserAndKweetId(user1.Id, kweet1.Id, out var like);
            Assert.IsTrue(value);
            Assert.AreSame(like1, like);
        }

        [TestMethod]
        public void TryGetLikeByUserAndKweetIdFalseFlowTest()
        {
            var value = _likeService.TryGetLikeByUserAndKweetId(user1.Id, kweet2.Id, out var like);
            Assert.IsFalse(value);
            Assert.IsNull(like);
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
    }
}
