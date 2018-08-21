using AutoMapper;
using Kwetter.Models;
using Kwetter.WebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetter.WebApplication.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            var kweetMapping = CreateMap<Kweet, KweetViewModel>();
            kweetMapping.ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.Likes != null && src.Likes.Count > 0 ? src.Likes.Count : 0));

            CreateMap<KweetViewModel, Kweet>();
            CreateMap<PostKweetViewModel, Kweet>();
            CreateMap<UpdateKweetViewModel, Kweet>();

            CreateMap<ApplicationUser, BaseUserViewModel>();
            CreateMap<BaseUserViewModel, ApplicationUser>().ForMember(u => u.Id, opt => opt.Ignore());
            CreateMap<ApplicationUser, UserViewModel>();

            CreateMap<ApplicationUser, ManagementUserViewModel>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserRoles.FirstOrDefault().Role.Name));
        }
    }
}
