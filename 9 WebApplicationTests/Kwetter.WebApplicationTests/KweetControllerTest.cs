using Kwetter.Data;
using Kwetter.Models;
using Kwetter.WebApplication.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwetter.WebApplicationTests
{
    [TestClass]
    public class KweetControllerTest : ControllerTestBase
    {
        [TestInitialize]
        public async Task Init()
        {
            
        }

        [TestMethod]
        public async Task GetAllNewestTest()
        {
            var kweets = await GetAsync<ICollection<KweetViewModel>>("/kweets", Role.Anonymous);
            Assert.AreEqual(2, kweets.Count);
        }

        [TestMethod]
        public async Task GetByIdTest()
        {
            var kweets = await GetAsync<KweetViewModel>("/kweets/1", Role.Anonymous);
            Assert.AreEqual("first kweet", kweets.Description);
        }

        [TestMethod]
        public async Task GetTimeLineTest()
        {
            var kweets = await GetAsync<ICollection<KweetViewModel>>("/kweets/timeline/1", Role.Anonymous);
            Assert.AreEqual(2, kweets.Count); 
        }

        [TestMethod]
        public async Task GetBySearchTest()
        {
            var kweets = await GetAsync<ICollection<KweetViewModel>>("/kweets/search/first", Role.Anonymous);
            Assert.AreEqual("first kweet", kweets.First().Description);
            Assert.AreEqual(1, kweets.Count);
        }
    }
}
