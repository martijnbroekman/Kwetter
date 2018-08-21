using AutoMapper;
using Kwetter.Models;
using Kwetter.Repository.Patterns;
using Kwetter.Service;
using Kwetter.WebApplication.Filters;
using Kwetter.WebApplication.Helpers.Attributes;
using Kwetter.WebApplication.Helpers.Extensions;
using Kwetter.WebApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.WebApplication.Controllers
{
    [JwtAuthorize]
    [Route("[controller]")]
    public class KweetsController : Controller
    {
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly IKweetService _kweetService;
        private readonly ILikeService _likeService;
        private readonly IHashTagService _hashTagService;
        private readonly IHashTagInKweetService _hashTagInKweetService;
        private readonly IMentionService _mentionService;
        private readonly IMapper _mapper;

        public KweetsController(IUnitOfWorkAsync unitOfWork, IKweetService kweetService, ILikeService likeService, IHashTagService hashTagService, IHashTagInKweetService hashTagInKweetService, IMentionService mentionService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _kweetService = kweetService;
            _likeService = likeService;
            _hashTagService = hashTagService;
            _hashTagInKweetService = hashTagInKweetService;
            _mentionService = mentionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostKweetViewModel kweet)
        {
            var model = _mapper.Map<PostKweetViewModel, Kweet>(kweet);
            model.UserId = User.GetId();
            model.Date = DateTime.Now;

            _kweetService.Insert(model);
            await _unitOfWork.SaveChangesAsync();

            var hashtags = _hashTagService.GetHashTagsFromKweetDescription(model.Description);
            var insertedHashtags = await _hashTagService.InsertRangeAsync(hashtags);
            await _unitOfWork.SaveChangesAsync();

            await _hashTagInKweetService.InsertRangeForKweet(model, insertedHashtags);
            await _unitOfWork.SaveChangesAsync();

            // ReSharper disable once Mvc.ActionNotResolved
            var uri = Url.Action("Kweets", new { id = model.Id });
            return Created(uri, MapKweet(await _kweetService.GetKweetWithLikesAsync(model.Id)));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNewest([FromQuery]RequestFilter filter)
        {
            if (filter.From > filter.To)
                return BadRequest($"{nameof(filter.From)} needs to be smaller than {nameof(filter.To)}");
            if (filter.To - filter.From > 40)
                return BadRequest("You cannot request more than 40 values");
            return Ok(MapKweetsCollection(await _kweetService.GetAllNewestAsync(filter.From, filter.To)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(MapKweet(await _kweetService.GetKweetWithLikesAsync(id)));
        }

        [HttpGet("timeline/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTimeLine([FromQuery]RequestFilter filter, int userId)
        {
            if (filter.From > filter.To)
                return BadRequest($"{nameof(filter.From)} needs to be smaller than {nameof(filter.To)}");
            if (filter.To - filter.From > 40)
                return BadRequest("You cannot request more than 40 values");
            return Ok(MapKweetsCollection(await _kweetService.GetTimeLineForUserIdAsync(userId, filter.From, filter.To)));
        }
        
        [HttpGet("newest/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewest([FromQuery]RequestFilter filter, int userId)
        {
            if (filter.From > filter.To)
                return BadRequest($"{nameof(filter.From)} needs to be smaller than {nameof(filter.To)}");
            if (filter.To - filter.From > 40)
                return BadRequest("You cannot request more than 40 values");
            return Ok(MapKweetsCollection(await _kweetService.GetAllNewestByUserIdAsync(userId, filter.From, filter.To)));
        }

        [HttpGet("search/{keyword}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySearch([FromQuery]RequestFilter filter, string keyword)
        {
            if (filter.From > filter.To)
                return BadRequest($"{nameof(filter.From)} needs to be smaller than {nameof(filter.To)}");
            if (filter.To - filter.From > 40)
                return BadRequest("You cannot request more than 40 values");
            return Ok(MapKweetsCollection(await _kweetService.SearchKweetAsync(keyword, filter.From, filter.To)));
        }

        [HttpDelete("{kweetId}")]
        public async Task<IActionResult> Delete(int kweetId)
        {
            if (kweetId <= 0)
                return BadRequest($"{nameof(kweetId)} cannot be smaller or equal to zero");
            if (!_kweetService.TryFind(kweetId, out var kweet))
                return NotFound($"No kweet found with the Id: {kweetId}");
            
            await _mentionService.RemoveRangeByKweetIdAsync(kweetId);
            await _hashTagInKweetService.RemoveRangeByKweetIdAsync(kweetId);
            await _likeService.RemoveRangeByKweetIdAsync(kweetId);
            await _unitOfWork.SaveChangesAsync();

            _kweetService.Remove(kweetId);
            await _unitOfWork.SaveChangesAsync();
            
            return Ok(kweetId);
        }

        [HttpPatch("{id}/Like")]
        public async Task<IActionResult> Like(int id)
        {
            var userId = User.GetId();
            if (_likeService.IsKweetLiked(userId, id))
            {
                return BadRequest("Kweet was allready liked");
            }
            _likeService.Insert(new Like
            {
                KweetId = id,
                UserId = userId
            });
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}/Like")]
        public async Task<IActionResult> Dislike(int id)
        {
            var userId = User.GetId();

            if (!_likeService.TryGetLikeByUserAndKweetId(userId, id, out var like))
            {
                return BadRequest("Kweet wasn't liked yet");
            }
            _likeService.Remove(like.Id);
#warning check delete voor hashtag
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.SaveChangesAsync();
            return Ok();
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
