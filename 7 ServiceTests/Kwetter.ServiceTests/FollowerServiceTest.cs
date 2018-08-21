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
    public class FollowerServiceTest : ServiceTestBase
    {
        private IUnitOfWorkAsync _unitOfWork;

        private IUserService _userService;
        private IFollowerService _followerService;

        private ApplicationUser user1;
        private ApplicationUser user2;

        [TestInitialize]
        public async Task Init()
        {

            _unitOfWork = new UnitOfWork(_context);
            _userService = new UserService(new UserRepository(_context, _unitOfWork));
            _followerService = new FollowerService(new Repository<Follower>(_context, _unitOfWork));

            await Seed();
        }

        #region TryFindByUserAndFollowsId tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryFindByUserAndFollowsIdShouldThrowArgumentExceptionForUserIdTest()
        {
            _followerService.TryFindByUserAndFollowsId(0, 1, out var follower);
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryFindByUserAndFollowsIdShouldThrowArgumentExceptionForFollowerIdTest()
        {
            _followerService.TryFindByUserAndFollowsId(1, 0, out var follower);
            Assert.Fail();
        }

        [TestMethod]
        public void TryFindByUserAndFollowsIdBadFlowTest()
        {
            var value = _followerService.TryFindByUserAndFollowsId(user1.Id, user2.Id, out var follower);
            Assert.IsFalse(value);
        }

        [TestMethod]
        public async Task TryFindByUserAndFollowsIdGoodFlowTest()
        {
            _followerService.Insert(new Follower
            {
                User = user1,
                UserId = user1.Id,
                Follows = user2,
                FollowsId = user2.Id
            });
            await _unitOfWork.SaveChangesAsync();
            var value = _followerService.TryFindByUserAndFollowsId(user1.Id, user2.Id, out var follower);
            Assert.IsTrue(value);
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
        }
    }
}
