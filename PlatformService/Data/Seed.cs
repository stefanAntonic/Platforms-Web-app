using PlatformService.Models;

namespace PlatformService.Data;

public class Seed
{
    private readonly AppDbContext _context;

    public Seed( AppDbContext  context)
    {
        _context = context;
    }

    public void SeedData()
    {
        if (!_context.Platforms.Any())
        {
            Console.WriteLine("Seeding Data");
            _context.Platforms.AddRange(
                new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "SQL Server", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Linux", Publisher = "Linux", Cost = "Free" }
            );
            _context.SaveChanges();
        } else 
        {
            Console.WriteLine("Data Already Exists");
        }
    }
}