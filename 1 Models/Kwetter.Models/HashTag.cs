using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetter.Models
{
    public class HashTag : IModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<HashTagInKweet> HashTagInKweets { get; set; }

        public HashTag()
        {
            HashTagInKweets = new List<HashTagInKweet>();
        }
    }
}
