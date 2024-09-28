using Microsoft.EntityFrameworkCore;

namespace EFCore.GenericRepository.Tests;

public class DemoDbContext : DbContext
{
    public DemoDbContext()
    {
    }

    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
    {
    }
}
