using CommandsService.Models;

namespace CommandsService.Interface;

public interface ICommandRepository
{
    bool SaveChanges();
    IEnumerable<Platform> GetAllPlatforms();
    bool CreatePlatform(Platform platform);
    bool PlatformExists(int platformId);
    IEnumerable<Command> GetPlatformCommands(int platformId);
    Command GetCommand(int platformId, int commandId);
    bool CreateCommand(int platformId, Command command);




}