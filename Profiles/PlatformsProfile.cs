using AutoMapper;
using PlatformWell.Dtos;
using PlatformWell.Models;

namespace PlatformWell.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            //Source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformUpdateDto, Platform>();
            CreateMap<Platform, PlatformUpdateDto>();
        }

    }
    
}