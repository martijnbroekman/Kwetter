using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kwetter.Models;
using Kwetter.Service;
using Kwetter.WebApplication.Helpers.Attributes;
using Kwetter.WebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kwetter.WebApplication.Controllers
{
    [JwtAuthorize(Roles = UserRoles.Moderator + "," + UserRoles.Administrator)]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[Action]")]
    public class KweetManagementController : Controller
    {
        private readonly IUserService _userService;
        private readonly IKweetService _kweetService;

        private readonly IMapper _mapper;
        
        public KweetManagementController(IUserService userService, IKweetService kweetService, IMapper mapper)
        {
            _userService = userService;
            _kweetService = kweetService;

            _mapper = mapper;
        }
        
        [HttpGet("{userId}")]
        public async Task<IActionResult> Index(int userId)
        {
            var user = _mapper.Map<ApplicationUser, UserViewModel>(await _userService.GetById(userId));
            var kweets = MapKweetsCollection(await _kweetService.GetAllNewestByUserIdAsync(userId, 0, 10));
            
            return View(new KweetManagementViewModel(user, kweets));
        }
        
        #region Helpers

        private KweetViewModel MapKweet(Kweet kweet)
        {
            var vm = _mapper.Map<Kweet, KweetViewModel>(kweet);
            if (int.TryParse(User.FindFirst("sub")?.Value, out int userId))
                vm.IsLiked = kweet.Likes.Any(l => l.UserId.Equals(userId));
            return vm;
        }

        private ICollection<KweetViewModel> MapKweetsCollection(ICollection<Kweet> kweets)
        {
            ICollection<KweetViewModel> vm = new List<KweetViewModel>();
            foreach (var kweet in kweets)
            {
                vm.Add(MapKweet(kweet));
            }
            return vm;
        }

        #endregion
    }
}