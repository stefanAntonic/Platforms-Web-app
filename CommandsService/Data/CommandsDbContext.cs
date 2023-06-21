using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data;

public class CommandsDbContext : DbContext
{
    public CommandsDbContext( DbContextOptions<CommandsDbContext> options): base(options)
    {
        
    }

    public DbSet<Command> Commands;
    public DbSet<Platform> Platforms;

}