// <copyright file="Repository.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace TanvirArjel.EFCore.GenericRepository
{
    internal class Repository<TDbContext> : QueryRepository<TDbContext>, IRepository, IRepository<TDbContext>
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        public Repository(TDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.Unspecified,
            CancellationToken cancellationToken = default)
        {
            IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return dbContextTransaction;
        }

        public async Task<object[]> InsertAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
           where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            EntityEntry<TEntity> entityEntry = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            object[] primaryKeyValue = entityEntry.Metadata.FindPrimaryKey().Properties.
                Select(p => entityEntry.Property(p.Name).CurrentValue).ToArray();

            return primaryKeyValue;
        }

        public async Task InsertAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
           where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<int> UpdateAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            EntityEntry<TEntity> trackedEntity = _dbContext.ChangeTracker.Entries<TEntity>().FirstOrDefault(x => x.Entity == entity);

            if (trackedEntity == null)
            {
                IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));

                if (entityType == null)
                {
                    throw new InvalidOperationException($"{typeof(TEntity).Name} is not part of EF Core DbContext model");
                }

                string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();

                if (primaryKeyName != null)
                {
                    Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

                    object primaryKeyDefaultValue = primaryKeyType.IsValueType ? Activator.CreateInstance(primaryKeyType) : null;

                    object primaryValue = entity.GetType().GetProperty(primaryKeyName).GetValue(entity, null);

                    if (primaryKeyDefaultValue.Equals(primaryValue))
                    {
                        throw new InvalidOperationException("The primary key value of the entity to be updated is not valid.");
                    }
                }

                _dbContext.Set<TEntity>().Update(entity);
            }

            int count = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public async Task<int> UpdateAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<T>().UpdateRange(entities);
            int count = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public async Task<int> DeleteAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Set<T>().Remove(entity);
            int count = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public async Task<int> DeleteAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<T>().RemoveRange(entities);
            int count = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken = default)
        {
            return _dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            return _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        public void ClearChangeTracker()
        {
            _dbContext.ChangeTracker.Clear();
        }

        public void Add<TEntity>(TEntity entity)
            where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Set<TEntity>().Add(entity);
        }

        public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
   where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }

        public void Add<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<TEntity>().AddRange(entities);
        }

        public async Task AddAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
   where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        }

        public void Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            EntityEntry<TEntity> trackedEntity = _dbContext.ChangeTracker
                .Entries<TEntity>().FirstOrDefault(x => x.Entity == entity);

            if (trackedEntity == null)
            {
                IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));

                if (entityType == null)
                {
                    throw new InvalidOperationException($"{typeof(TEntity).Name} is not part of EF Core DbContext model");
                }

                string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();

                if (primaryKeyName != null)
                {
                    Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

                    object primaryKeyDefaultValue = primaryKeyType.IsValueType ? Activator.CreateInstance(primaryKeyType) : null;

                    object primaryValue = entity.GetType().GetProperty(primaryKeyName).GetValue(entity, null);

                    if (primaryKeyDefaultValue.Equals(primaryValue))
                    {
                        throw new InvalidOperationException("The primary key value of the entity to be updated is not valid.");
                    }
                }

                _dbContext.Set<TEntity>().Update(entity);
            }
        }

        public void Update<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<TEntity>().UpdateRange(entities);
        }

        public void Remove<TEntity>(TEntity entity)
            where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void Remove<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            _dbContext.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            int count = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return count;
        }
    }
}