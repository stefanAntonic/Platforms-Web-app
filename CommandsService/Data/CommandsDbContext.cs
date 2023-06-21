using CommandsService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CommandsService.Data;

public class CommandsDbContext : DbContext
{
    public CommandsDbContext( DbContextOptions<CommandsDbContext> options): base(options)
    {
        
    }

    public DbSet<Command> Commands { get; set; }
    public DbSet<Platform> Platforms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Platform>()
            .HasMany(p => p.Commands)
            .WithOne(p => p.Platform)
            .HasForeignKey(p => p.PlatformId);
        modelBuilder
            .Entity<Command>()
            .HasOne(p => p.Platform)
            .WithMany(p => p.Commands)
            .HasForeignKey(p => p.PlatformId);

    }
}