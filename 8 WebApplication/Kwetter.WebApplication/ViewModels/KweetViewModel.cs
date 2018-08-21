using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.WebApplication.ViewModels
{
    public class PostKweetViewModel
    {
        [MaxLength(140)]
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }

    public class UpdateKweetViewModel : PostKweetViewModel
    {
        public int Id { get; set; }
    }

    public class KweetViewModel : UpdateKweetViewModel
    {
        public bool IsLiked { get; set; }
        public int LikesCount { get; set; }
        public BaseUserViewModel User { get; set; }
    }

    
}
