using AutoMapper;
using HRMSPOC.API.DTOs;
using HRMSPOC.API.Models;

namespace HRMSPOC.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           // CreateMap<ApplicationUserDto, ApplicationUser>().ForMember(dest => dest.Id, opt => opt.Ignore());
           CreateMap<ApplicationUserDto, ApplicationUser>();
            CreateMap<ApplicationUser,CreateUserDto>();
            CreateMap<CreateUserDto,ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<Organization, OrganizationDto>().ReverseMap();
            CreateMap<UserOrganization, UserOrganizationDto>().ReverseMap();
        }
    }
}
