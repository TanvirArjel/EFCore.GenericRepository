using System;
using Microsoft.EntityFrameworkCore;

namespace TanvirArjel.EFCore.GenericRepository
{
    internal sealed class QueryRepositoryFactory<TContext> : IQueryRepositoryFactory<TContext>
        where TContext : DbContext
    {
        private readonly IDbContextFactory<TContext> _dbContextFactory;

        public QueryRepositoryFactory(IDbContextFactory<TContext> dbContextFactory) => 
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));

        public IQueryRepository<TContext> CreateQueryRepository()
        {
            TContext dbContext = _dbContextFactory.CreateDbContext();
            return new QueryRepository<TContext>(dbContext);
        }
    }
}