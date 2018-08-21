using AutoMapper;
using Kwetter.Models;
using Kwetter.Repository.Patterns;
using Kwetter.Service;
using Kwetter.WebApplication.Filters;
using Kwetter.WebApplication.Helpers.Attributes;
using Kwetter.WebApplication.Helpers.Extensions;
using Kwetter.WebApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.WebApplication.Controllers
{
    [Route("[controller]")]
    [JwtAuthorize]
    public class UsersController : Controller
    {
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly IUserService _userService;
        private readonly IFollowerService _followerService;
        private readonly ILikeService _likeService;
        private readonly IHashTagInKweetService _hashTagInKweetService;
        private readonly IMentionService _mentionService;
        private readonly IUserRoleService _userRoleService;
        
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWorkAsync unitOfWork, IUserService userService, IFollowerService followerService, ILikeService likeService, IHashTagInKweetService hashTagInKweetService, IMentionService mentionService, IUserRoleService userRoleService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _followerService = followerService;
            _likeService = likeService;
            _hashTagInKweetService = hashTagInKweetService;
            _mentionService = mentionService;
            _userRoleService = userRoleService;

            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery]RequestFilter filter)
        {
            if (filter.From > filter.To)
                return BadRequest("From needs to be smaller than To");
            if (filter.To - filter.From > 40)
                return BadRequest("You cannot request more than 40 values");

            var users = await _userService.GetRange(filter.From, filter.To);

            return Ok(_mapper.Map<ICollection<ApplicationUser>, ICollection<UserViewModel>>(users));
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Moderator + "," + UserRoles.Administrator)]
        [Route("withRoles")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllWithRoles();

            return Ok(_mapper.Map<ICollection<ApplicationUser>, ICollection<ManagementUserViewModel>>(users));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            if (id <= 0)
                return BadRequest("The id cannot be lower than 1");
            if (!_userService.TryFind(id, out var user))
                return NotFound($"There isn't a user with the id: \"{id}\"");
            return Ok(_mapper.Map<ApplicationUser, UserViewModel>(user));
        }

        [HttpPatch]
        public IActionResult Update([FromBody]BaseUserViewModel user)
        {
            var id = User.GetId();
            if (!user.Id.Equals(id))
                return BadRequest("You can only update your own account");
            if (!_userService.TryFind(id, out var currentUser))
                return NotFound("There isn't a user with that id");
            _mapper.Map(user, currentUser);
            _userService.Update(currentUser);
            _unitOfWork.SaveChanges();

            return Ok(user);
        }

        [HttpDelete]
        [Route("{userId}")]
        [Authorize(Roles = UserRoles.Moderator + "," + UserRoles.Administrator)]
        public async Task<IActionResult> Delete(int userId)
        {
            await _likeService.RemoveRangeByUserIdAsync(userId);
            await _hashTagInKweetService.RemoveRangeByUserIdAsync(userId);
            await _followerService.RemoveRangeByUserIdAsync(userId);
            await _mentionService.RemoveRangeByUserIdAsync(userId);

            await _unitOfWork.SaveChangesAsync();

            var isRemoved = _userService.Remove(userId);
            await _unitOfWork.SaveChangesAsync();

            return Ok(isRemoved);
        }

        [HttpGet("{userId}/followers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFollowers(int userId)
        {
            if (userId <= 0)
                return BadRequest($"{nameof(userId)} must be lower than 1");
            var followers = await _userService.GetFollowers(userId);

            return Ok(_mapper.Map<ICollection<ApplicationUser>, ICollection<BaseUserViewModel>>(followers));
        }

        [HttpPatch("{userId}/following")]
        public IActionResult Follow(int userId)
        {
            if (userId <= 0)
                return BadRequest($"{nameof(userId)} must be lower than 1");
            if (userId.Equals(User.GetId()))
                return BadRequest($"You can't follow yourself");
            if (_followerService.TryFindByUserAndFollowsId(User.GetId(), userId, out var newFollower))
                return BadRequest($"{userId} is allready being followed");
            var follower = new Follower
            {
                Date = DateTime.Now,
                UserId = User.GetId(),
                FollowsId = userId
            };
            _followerService.Insert(follower);
            _unitOfWork.SaveChanges();

            return Ok();
        }

        [HttpDelete("{userId}/following")]
        public IActionResult UnFollow(int userId)
        {
            if (userId <= 0)
                return BadRequest($"{nameof(userId)} must be lower than 1");
            if (!_followerService.TryFindByUserAndFollowsId(User.GetId(), userId, out var follower))
                return NotFound($"The user doesn't follow a user with Id {userId}");
            _followerService.Remove(follower.Id);
            _unitOfWork.SaveChanges();

            return Ok();
        }

        [HttpGet("{userId}/following")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFollowing(int userId)
        {
            if (userId <= 0)
                return BadRequest($"{nameof(userId)} must be lower than 1");
            var following = await _userService.GetFollowing(userId);

            return Ok(_mapper.Map<ICollection<ApplicationUser>, ICollection<BaseUserViewModel>>(following));
        }

        [HttpPatch]
        [Route("{userId}/role")]
        [Authorize(Roles = UserRoles.Administrator)]
        public async Task<IActionResult> UpdateRole(int userId, [FromBody]RoleViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest("Role cannot be null");
            if (userId.Equals(User.GetId()))
                return BadRequest("You cannot update your own role");
            if (!_userService.TryFind(userId, out var user))
                return NotFound($"There is no user with {nameof(userId)}");
            if (!await _userRoleService.SetRole(userId, vm.Role))
                return BadRequest($"{user.UserName} allready has {vm.Role}");
            await _unitOfWork.SaveChangesAsync();        
            
            return Ok(new { roleSet = true });
        }
        
        [HttpPatch]
        [Route("{userId}/IsBanned")]
        [Authorize(Roles = UserRoles.Administrator + "," + UserRoles.Moderator)]
        public async Task<IActionResult> UpdateIsBanned(int userId, [FromBody]IsBannedViewModel vm)
        {
            if (userId.Equals(User.GetId()))
                return BadRequest("You cannot update your own banning");
            if (!_userService.TryFind(userId, out var user))
                return NotFound($"There is no user with {nameof(userId)}");
            user.IsBanned = vm.IsBanned;
            _userService.Update(user);
            await _unitOfWork.SaveChangesAsync();
            
            return Ok(new { roleSet = true });
        }
    }
}
