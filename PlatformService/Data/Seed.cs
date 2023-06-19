using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public class Seed
{
#pragma warning disable CS8618
    private static AppDbContext _context;
#pragma warning restore CS8618

    public Seed( AppDbContext  context)
    {
        _context = context;
    }

    public void SeedData(bool production)
    {
        if (production)
        {
            try
            {
                Console.WriteLine("Db production ready");
                _context.Database.Migrate();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
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