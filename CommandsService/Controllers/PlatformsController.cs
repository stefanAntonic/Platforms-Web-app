using AutoMapper;
using CommandsService.Dto;
using CommandsService.Interface;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CommandsService.Controllers;

[Route("api/controllers/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;

    public PlatformsController(ICommandRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Platform>))]
    public IActionResult GetPlatforms()
    {
        Console.WriteLine("--> Getting platfroms");
        var platforms = _mapper.Map<List<PlatformReadDto>>(_repository.GetAllPlatforms());
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        return Ok(platforms);
    }



    [HttpPost]
    public IActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound POST # Command Service");
        return Ok("Inbound test of from Platforms Controller");
    }
}