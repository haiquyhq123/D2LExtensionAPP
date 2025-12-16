using AutoMapper;
using D2LExtensionWebAPPSSR.Models;

namespace D2LExtensionWebAPPSSR.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationModel, User>().ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
