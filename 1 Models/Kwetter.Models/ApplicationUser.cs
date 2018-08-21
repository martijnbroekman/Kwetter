using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Kwetter.Models
{
    public class ApplicationUser : IdentityUser<int>, IModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool IsBanned { get; set; }


        public ICollection<Like> Likes { get; set; }
        public ICollection<Mention> Mentions { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public ICollection<Follower> Following { get; set; }
        public ICollection<Follower> Followers { get; set; }
        public ICollection<Kweet> Kweets { get; set; }

        public ApplicationUser()
        {
            UserRoles = new List<ApplicationUserRole>();
            Likes = new List<Like>();
            Mentions = new List<Mention>();
            Followers = new List<Follower>();
            Following = new List<Follower>();
            Kweets = new List<Kweet>();
        }
    }
}
