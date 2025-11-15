using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

[assembly: InternalsVisibleTo("EFCore.GenericRepository.Tests")]

namespace TanvirArjel.EFCore.GenericRepository
{
    internal sealed class RepositoryFactory<TDbContext> : IRepositoryFactory<TDbContext>
        where TDbContext : DbContext
    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;

        public RepositoryFactory(IDbContextFactory<TDbContext> dbContextFactory) =>
            _dbContextFactory = dbContextFactory ?? throw new System.ArgumentNullException(nameof(dbContextFactory));

        public IRepository<TDbContext> CreateRepository()
        {
            TDbContext dbContext = _dbContextFactory.CreateDbContext();
            return new Repository<TDbContext>(dbContext);
        }
    }
}