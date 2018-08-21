using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetter.Models
{
    public class HashTagInKweet : IModel
    {
        public int Id { get; set; }
        public HashTag HashTag { get; set; }
        public int HashTagId { get; set; }
        public Kweet Kweet { get; set; }
        public int KweetId { get; set; }
    }
}
