using System;
using AspNetCore5._0.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore5._0.Data
{
    public class DemoDbContext : DbContext
    {
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
            modelBuilder.Entity<Employee>().HasKey(e => e.EmployeeId);
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistories { get; set; }
    }

    public class DemoDbContext2 : DbContext
    {
        public DemoDbContext2(DbContextOptions<DemoDbContext2> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasKey(e => e.EmployeeId);
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeHistory> EmployeeHistories { get; set; }
    }
}
