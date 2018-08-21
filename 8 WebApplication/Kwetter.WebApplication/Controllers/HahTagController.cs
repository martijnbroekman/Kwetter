using AutoMapper;
using Kwetter.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.WebApplication.Controllers
{
    [Route("[controller]")]
    public class HahTagController : Controller
    {
        private readonly IHashTagService _hashTagService;

        public HahTagController(IHashTagService hashTagService)
        {
            _hashTagService = hashTagService;
        }

        [HttpGet("{amount}")]
        public async Task<IActionResult> GetTop(int amount = 10)
        {
            return Ok((await _hashTagService.GetTopAsync(amount)).Select(h => h.Title));
        }
    }
}
