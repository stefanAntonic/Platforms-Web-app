using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dto;
using PlatformService.Interface;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformController : ControllerBase
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;

    public PlatformController(
        IPlatformRepository repository,
        IMapper  mapper,
        ICommandDataClient commandDataClient
    )
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Platform>))]
    public IActionResult GetPlatforms()
    {
        var platforms = _mapper.Map<List<PlatformDto>>(_repository.GetAllPlatforms());

        if (!ModelState.IsValid)
        {
            return BadRequest();    
        }
        
        return Ok(platforms);
        
    }

    [HttpGet("{id}", Name = "GetPlatform")]
    [ProducesResponseType(200, Type = typeof(Platform))]
    [ProducesResponseType(400)]
    public IActionResult GetPlatform(int id)
    {
        var platform = _mapper.Map<PlatformDto>(_repository.GetPlatformById(id));

        if (platform == null)
        { 
            return NotFound();
        }
        if(!ModelState.IsValid && platform.GetType().GetProperty("Id") == null)
        {
            return BadRequest();
        }
        return Ok(platform);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async  Task<IActionResult> CreatePlatform([FromBody] PlatformDto? platform)
    {
            if (platform == null)
            {
                return BadRequest();
            }
            var isPlatformExists = _repository
                .GetAllPlatforms()
                .FirstOrDefault(p => p.Name.Trim().ToLower() == platform.Name.Trim().ToLower());
            if (isPlatformExists != null)
            {
                ModelState.AddModelError("Platform", "Platform already exists");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
            {
                    return BadRequest();    
            }
            var platformMap =  _mapper.Map<Platform>(platform);
            try
            {
                await _commandDataClient.SendPlatformToCommand(_mapper.Map<PlatformDto>(platformMap));
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Cloud not send synchronously: {e.Message} ");
                throw;
            }
            if(!_repository.CreatePlatform(platformMap))
            {
                ModelState.AddModelError("Platform", "Something went wrong while creating platform");
                return StatusCode(500, ModelState);
            }

            // return Ok("Successfully Created")
            return CreatedAtRoute("GetPlatform", new { id = platformMap.Id }, platformMap);
    }
}