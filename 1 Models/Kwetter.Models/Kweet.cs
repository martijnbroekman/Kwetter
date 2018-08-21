using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetter.Models
{
    public class Kweet : IModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Like> Likes { get; set; }

        public Kweet()
        {
            Likes = new List<Like>();
        }
    }
}
