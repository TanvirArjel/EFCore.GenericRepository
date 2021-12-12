using AspNetCore3._1.Data.Models;
using AspNetCore3._1.Data.Models.Abstact;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AspNetCore3._1.Data
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));

            var baseEntityType = typeof(BaseEntity);
            var entitiesAssembly = baseEntityType.Assembly;
            var allTypes = entitiesAssembly.GetTypes();
            var entities = allTypes.Where(q => q.BaseType == baseEntityType && q != baseEntityType).ToList();
            foreach (var entityType in entities)
            {
                UseAsEntity(modelBuilder, entityType);
            }


            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasKey(e => e.EmployeeId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        private static void UseAsEntity(ModelBuilder modelBuilder, Type type)
        {
            modelBuilder.Entity(type);
        }
    }
}
