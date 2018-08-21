using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.WebApplication.ViewModels
{
    public class BaseUserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
    }

    public class ManagementUserViewModel : BaseUserViewModel
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string LockoutEnabled { get; set; }
        public bool IsBanned { get; set; }
    }

    public class UserViewModel : BaseUserViewModel
    {
    }
}
