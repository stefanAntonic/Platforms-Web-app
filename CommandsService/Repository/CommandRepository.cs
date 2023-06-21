using CommandsService.Data;
using CommandsService.Interface;
using CommandsService.Models;

namespace CommandsService.Repository;

public class CommandRepository : ICommandRepository
{
    private readonly CommandsDbContext _context;

    public CommandRepository(CommandsDbContext context)
    {
        _context = context;
    }

    public bool SaveChanges()
    {
        return (_context.SaveChanges() >= 0);
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _context.Platforms.ToList();
    }

    public bool CreatePlatform(Platform platform)
    {
            if (platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _context.Platforms.Add(platform);
            return SaveChanges();
    }

    public bool PlatformExists(int platformId)
    {
        return _context.Platforms.Any(plat => plat.Id == platformId);
    }

    public IEnumerable<Command> GetPlatformCommands(int platformId)
    {
        return _context
            .Commands
            .Where(command => command.PlatformId == platformId)
            .OrderBy(command => command.Platform.Name)
            .ToList();
    }

    public Command GetCommand(int platformId, int commandId)
    {
        return _context
            .Commands
            .FirstOrDefault(command => command.Id == commandId && command.PlatformId == platformId);
    }

    public bool CreateCommand(int platformId, Command command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));

        }

        command.PlatformId = platformId;
        var platform = _context
            .Platforms
            .FirstOrDefault(plat => plat.Id == platformId);
        platform.Commands.Add(command);
        command.Platform = platform;

        _context.Platforms.Update(platform);
        _context.Commands.Add(command);
        return SaveChanges();
    }
}