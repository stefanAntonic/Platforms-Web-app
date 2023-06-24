using System.ComponentModel.Design;
using AutoMapper;
using CommandsService.Dto;
using CommandsService.Interface;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CommandsService.Controllers;


[Route("api/controllers/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CommandReadDto>))]
    public IActionResult GetCommandsForPlatform(int platformId)
    {
        var platform = _repository.PlatformExists(platformId);
        if (!platform)
        {
            ModelState.AddModelError("", "Platform doesn't exist.");
            return StatusCode(442, ModelState);
        };
        
        var commands = _mapper.Map<List<CommandReadDto>>(_repository.GetPlatformCommands(platformId));
        if (!ModelState.IsValid)
        {
            return BadRequest();
            
        }
        return Ok(commands);
    }

    [HttpGet("{commandId}")]
    [ProducesResponseType(200, Type = typeof(CommandReadDto))]
    public IActionResult GetCommandForPlatform(int platformId, int commandId)
    {
        var command = _mapper.Map<CommandReadDto>(_repository.GetCommand(platformId, commandId));
        if (command == null)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return BadRequest();
            
        }
        return Ok(command);
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(PlatformReadDto))]
    public IActionResult CreateCommand(int platformId, [FromBody] CommandCreateDto commandCreateDto)
    {
        var platformExists = _repository.PlatformExists(platformId);
        if (!platformExists)
        {
            ModelState.AddModelError("", "Platform doesn't exists");
            return StatusCode(442, ModelState);
        };
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var command = _mapper.Map<Command>(commandCreateDto);
        if (!_repository.CreateCommand(platformId, command))
        {
            ModelState.AddModelError("", "Something went wrong while saving command");
            
        }

        var createdCommand = _mapper.Map<CommandReadDto>(command);

        return CreatedAtRoute(nameof(GetCommandForPlatform), 
            new { platformId, commandId = createdCommand.Id, createdCommand });

    }
}