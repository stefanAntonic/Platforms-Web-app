using PlatformService.Models;

namespace PlatformService.Interface;

public interface IPlatformRepository
{
    bool SaveChanges();
    
    IEnumerable<Platform> GetAllPlatforms();
    
    Platform GetPlatformById(int id);
    
    bool CreatePlatform(Platform platform);
}