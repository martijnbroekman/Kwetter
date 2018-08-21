using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetter.Models
{
    public class Mention : IModel
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public int UserId { get; set; }
        public Kweet Kweet { get; set; }
        public int KweetId { get; set; }
    }
}
