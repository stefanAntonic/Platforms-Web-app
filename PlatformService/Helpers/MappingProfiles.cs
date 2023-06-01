using AutoMapper;
using PlatformService.Dto;
using PlatformService.Models;

namespace PlatformService.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Platform, PlatformDto>().ReverseMap();
    }
}