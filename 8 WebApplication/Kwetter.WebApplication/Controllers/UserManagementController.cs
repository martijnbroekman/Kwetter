using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kwetter.Models;
using Kwetter.Service;
using Kwetter.WebApplication.Helpers.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Kwetter.WebApplication.Controllers
{
    [JwtAuthorize(Roles = UserRoles.Moderator + "," + UserRoles.Administrator)]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[Action]")]
    public class UserManagementController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagementController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


    }
}