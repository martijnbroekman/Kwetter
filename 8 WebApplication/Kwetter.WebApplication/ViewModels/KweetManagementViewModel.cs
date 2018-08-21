using System.Collections;
using System.Collections.Generic;

namespace Kwetter.WebApplication.ViewModels
{
    public class KweetManagementViewModel
    {
        public UserViewModel User { get; set; }
        public ICollection<KweetViewModel> Kweets { get; set; }

        public KweetManagementViewModel(UserViewModel user, ICollection<KweetViewModel> kweets)
        {
            User = user;
            Kweets = kweets;
        }
    }
}