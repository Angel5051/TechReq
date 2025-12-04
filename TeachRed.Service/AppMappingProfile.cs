using AutoMapper;
using TeachRed.Domain.Models;
using TeachRed.Domain.ViewModels.LoginAndRegistration;
using TechReq.Domain.ModelsDb;

namespace TeachRed.Service
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Users, UsersDb>().ReverseMap();
            CreateMap<Users, LoginViewModel>().ReverseMap();
            CreateMap<Users, RegisterViewModel>().ReverseMap();
            CreateMap<RegisterViewModel, ConfirmEmailViewModel>().ReverseMap();
            CreateMap<Users, ConfirmEmailViewModel>().ReverseMap();

        }
    }
}
