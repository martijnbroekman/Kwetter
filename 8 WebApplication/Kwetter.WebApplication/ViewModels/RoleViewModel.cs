using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.WebApplication.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string Role { get; set; }
    }
}
