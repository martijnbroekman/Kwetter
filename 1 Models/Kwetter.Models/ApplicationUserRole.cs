using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetter.Models
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public ApplicationUser User { get; set; }
        public override int UserId { get; set; }
        public ApplicationRole Role { get; set; }
        public override int RoleId { get; set; }
    }
}
