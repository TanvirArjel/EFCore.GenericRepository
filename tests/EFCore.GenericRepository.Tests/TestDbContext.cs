using Microsoft.EntityFrameworkCore;

namespace EFCore.GenericRepository.Tests.TestModels;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<TestEntity> TestEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TestEntity>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<TestEntity>()
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<TestEntity>()
            .Property(e => e.Description)
            .HasMaxLength(500);
    }
}

public class TestEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Value { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

