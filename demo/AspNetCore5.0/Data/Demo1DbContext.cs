using System;
using AspNetCore5._0.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore5._0.Data
{
    public class Demo1DbContext : DbContext
    {
        public Demo1DbContext(DbContextOptions<Demo1DbContext> options) : base(options)
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
