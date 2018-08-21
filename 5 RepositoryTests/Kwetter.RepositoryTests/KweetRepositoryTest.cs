using Kwetter.Models;
using Kwetter.Repository;
using Kwetter.Repository.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.RepositoryTests
{
    [TestClass]
    public class KweetRepositoryTest : RepositoryTestBase
    {
        private IUnitOfWorkAsync _unitOfWork;

        private IKweetRepository _kweetRepository;
        private IUserRepository _userRepository;
        private IRepositoryAsync<Like> _likeRepository;

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

            await Seed();
        }

        [TestMethod]
        public async Task GetAllNewestAsyncTest()
        {
            var kweets = await _kweetRepository.GetNewestKweetsAsync(0, 1);
            Assert.AreEqual(1, kweets.Count);
            Assert.AreSame(kweets.ElementAt(0), kweet2);

            kweets = await _kweetRepository.GetNewestKweetsAsync(0, 2);
            Assert.AreEqual(2, kweets.Count);
            Assert.AreSame(kweets.ElementAt(0), kweet2);
            Assert.AreSame(kweets.ElementAt(1), kweet1);
        }

        [TestMethod]
        public async Task GetNewestKweetsByQueryAsyncTest()
        {
            var kweets = await _kweetRepository.GetNewestKweetsByQueryAsync((k => true), 0, 1);
            Assert.AreEqual(1, kweets.Count);
            Assert.AreSame(kweets.ElementAt(0), kweet2);

            kweets = await _kweetRepository.GetNewestKweetsByQueryAsync((k => true), 0, 2);
            Assert.AreEqual(2, kweets.Count);
            Assert.AreSame(kweets.ElementAt(0), kweet2);
            Assert.AreSame(kweets.ElementAt(1), kweet1);
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
    }
}
