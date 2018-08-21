using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetter.Models
{
    public class Follower : IModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ApplicationUser User { get; set; }
        public int UserId { get; set; }
        public ApplicationUser Follows { get; set; }
        public int FollowsId { get; set; }
    }
}
