using Microsoft.EntityFrameworkCore;

namespace EFCore.QueryRepository.Tests;

public class DemoDbContext : DbContext
{
    public DemoDbContext()
    {
    }

    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Employee>().HasKey(e => e.Id);
    }

    public virtual DbSet<Employee> Employees { get; set; }
}

public class Employee
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public string Name { get; set; }

    public Department Department { get; set; }
}

public class Department
{
    public int Id { get; set; }

    public string Name { get; set; }
}
