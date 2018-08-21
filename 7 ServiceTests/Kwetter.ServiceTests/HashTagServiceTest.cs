using Kwetter.Models;
using Kwetter.Repository;
using Kwetter.Repository.Patterns;
using Kwetter.Service;
using Kwetter.Service.Patterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.ServiceTests
{
    [TestClass]
    public class HashTagServiceTest : ServiceTestBase
    {
        private IUnitOfWorkAsync _unitOfWork;

        private IHashTagService _hashTagService;
        private IKweetService _kweetService;
        private IService<HashTagInKweet> _hashTagInKweetService;

        private ApplicationUser user1;

        private Kweet kweet1;
        private Kweet kweet2;
        private Kweet kweet3;

        private HashTag hashTag1;
        private HashTag hashTag2;
        private HashTag hashTag3;

        [TestInitialize]
        public async Task Init()
        {
            _unitOfWork = new UnitOfWork(_context);
            _hashTagService = new HashTagService(new HashTagRepository(_context, _unitOfWork));
            _kweetService = new KweetService(new KweetRepository(_context, _unitOfWork));
            _hashTagInKweetService = new Service<HashTagInKweet>(new Repository<HashTagInKweet>(_context, _unitOfWork));

            await Seed();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetTopAsyncShouldThrowArgumentExceptionForAmountTest()
        {
            await _hashTagService.GetTopAsync(0);
            Assert.Fail();
        }

        [TestMethod]
        public async Task GetTopAsyncTest()
        {
            var tops = (await _hashTagService.GetTopAsync(3)).ToList();

            Assert.AreEqual(3, tops.Count());
            Assert.AreSame(hashTag2, tops[0]);
            Assert.AreSame(hashTag1, tops[1]);
            Assert.AreSame(hashTag3, tops[2]);
        }

        [TestMethod]
        public async Task GetTopToLargeAsyncTest()
        {
            var tops = (await _hashTagService.GetTopAsync(4));

            Assert.AreEqual(3, tops.Count());
        }

        [TestMethod]
        public async Task GetTopSmallValueAsyncTest()
        {
            var tops = (await _hashTagService.GetTopAsync(2));

            Assert.AreEqual(2, tops.Count());
        }

        [TestMethod]
        public void GetHashTagsFromKweetDescriptionTest()
        {
            var matches = _hashTagService.GetHashTagsFromKweetDescription(@"test #test #test #test2").ToList();
            Assert.AreEqual(2, matches.Count);
            Assert.AreEqual("#test", matches[0].Title);
            Assert.AreEqual("#test2", matches[1].Title);
        }

        private async Task Seed()
        {
            user1 = new ApplicationUser
            {
                UserName = "user1",
                FirstName = "user 1",
                LastName = "lastname"
            };
            await _context.AddAsync(user1);
            await _context.SaveChangesAsync();

            kweet1 = new Kweet
            {
                Date = DateTime.Now.AddDays(-6),
                Description = "Description 1",
                User = user1,
                UserId = user1.Id
            };

            kweet2 = new Kweet
            {
                Date = DateTime.Now.AddDays(-6),
                Description = "Description 2",
                User = user1,
                UserId = user1.Id
            };

            kweet3 = new Kweet
            {
                Date = DateTime.Now.AddDays(-8),
                Description = "Description 3",
                User = user1,
                UserId = user1.Id
            };
            _kweetService.Insert(kweet2);
            _kweetService.Insert(kweet1);
            await _unitOfWork.SaveChangesAsync();

            hashTag1 = new HashTag
            {
                Title = "HashTag 1"
            };

            hashTag2 = new HashTag
            {
                Title = "HashTag 2"
            };

            hashTag3 = new HashTag
            {
                Title = "HashTag 3"
            };
            _hashTagService.Insert(hashTag1);
            _hashTagService.Insert(hashTag2);
            _hashTagService.Insert(hashTag3);
            await _unitOfWork.SaveChangesAsync();

            var hashTagInKweet1 = new HashTagInKweet
            {
                HashTag = hashTag1,
                HashTagId = hashTag1.Id,
                Kweet = kweet1,
                KweetId = kweet1.Id
            };

            var hashTagInKweet2 = new HashTagInKweet
            {
                HashTag = hashTag2,
                HashTagId = hashTag2.Id,
                Kweet = kweet1,
                KweetId = kweet1.Id
            };

            var hashTagInKweet3 = new HashTagInKweet
            {
                HashTag = hashTag3,
                HashTagId = hashTag3.Id,
                Kweet = kweet1,
                KweetId = kweet1.Id
            };

            var hashTagInKweet4 = new HashTagInKweet
            {
                HashTag = hashTag2,
                HashTagId = hashTag2.Id,
                Kweet = kweet2,
                KweetId = kweet2.Id
            };

            var hashTagInKweet5 = new HashTagInKweet
            {
                HashTag = hashTag1,
                HashTagId = hashTag1.Id,
                Kweet = kweet3,
                KweetId = kweet3.Id
            };

            _hashTagInKweetService.Insert(hashTagInKweet1);
            _hashTagInKweetService.Insert(hashTagInKweet2);
            _hashTagInKweetService.Insert(hashTagInKweet3);
            _hashTagInKweetService.Insert(hashTagInKweet4);
            _hashTagInKweetService.Insert(hashTagInKweet5);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
