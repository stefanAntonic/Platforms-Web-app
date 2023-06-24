using AutoMapper;
using CommandsService.Dto;
using CommandsService.Models;

namespace CommandsService.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Platform, PlatformReadDto>();
    }
    
}