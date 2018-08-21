using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.WebApplication.Filters
{
    public class RequestFilter
    {
        [Range(0, int.MaxValue, ErrorMessage = "{0} must be larger than 0")]
        public int From { get; set; } = 0;
        [Range(0, int.MaxValue, ErrorMessage = "{0} must be larger than 0")]
        public int To { get; set; } = 10;
    }
}
