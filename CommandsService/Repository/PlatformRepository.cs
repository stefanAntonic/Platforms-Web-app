using CommandsService.Data;
using CommandsService.Interface;

namespace CommandsService.Repository;

public class PlatformRepository : IPlatformRepository
{
    private readonly CommandsDbContext _context;

    public PlatformRepository(CommandsDbContext context)
    {
        _context = context;
    }
}