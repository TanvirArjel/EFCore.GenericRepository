// <copyright file="Repository.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace TanvirArjel.EFCore.GenericRepository.Implementations
{
    internal class Repository : IRepository
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
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

        public IQueryable<T> GetQueryable<T>()
            where T : class
        {
            return _dbContext.Set<T>();
        }

        [Obsolete]
        public async Task<List<T>> GetEntityListAsync<T>(bool asNoTracking = false)
            where T : class
        {
            return await GetListAsync<T>(false);
        }

        public async Task<List<T>> GetListAsync<T>()
            where T : class
        {
            return await GetListAsync<T>(false);
        }

        public async Task<List<T>> GetListAsync<T>(bool asNoTracking)
            where T : class
        {
            Func<IQueryable<T>, IIncludableQueryable<T, object>> nullValue = null;
            return await GetListAsync<T>(nullValue, asNoTracking);
        }

        public async Task<List<T>> GetListAsync<T>(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
            where T : class
        {
            return await GetListAsync<T>(includes, false);
        }

        public async Task<List<T>> GetListAsync<T>(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes, bool asNoTracking)
            where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            List<T> entities = await query.ToListAsync();

            return entities;
        }

        [Obsolete]
        public async Task<List<T>> GetEntityListAsync<T>(Expression<Func<T, bool>> condition, bool asNoTracking = false)
             where T : class
        {
            return await GetListAsync(condition, asNoTracking);
        }

        public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> condition)
             where T : class
        {
            return await GetListAsync(condition, false);
        }

        public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> condition, bool asNoTracking)
             where T : class
        {
            return await GetListAsync(condition, null, asNoTracking);
        }

        public async Task<List<T>> GetListAsync<T>(
            Expression<Func<T, bool>> condition,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            bool asNoTracking)
             where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            List<T> entities = await query.ToListAsync();

            return entities;
        }

        [Obsolete]
        public async Task<List<T>> GetEntityListAsync<T>(Specification<T> specification, bool asNoTracking = false)
            where T : class
        {
            return await GetListAsync(specification, asNoTracking);
        }

        public async Task<List<T>> GetListAsync<T>(Specification<T> specification)
           where T : class
        {
            return await GetListAsync(specification, false);
        }

        public async Task<List<T>> GetListAsync<T>(Specification<T> specification, bool asNoTracking)
           where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        [Obsolete]
        public async Task<List<TProjectedType>> GetProjectedEntityListAsync<T, TProjectedType>(
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            List<TProjectedType> entities = await _dbContext.Set<T>().Select(selectExpression).ToListAsync();

            return entities;
        }

        public async Task<List<TProjectedType>> GetProjectedListAsync<T, TProjectedType>(
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            List<TProjectedType> entities = await _dbContext.Set<T>().Select(selectExpression).ToListAsync();

            return entities;
        }

        [Obsolete]
        public async Task<List<TProjectedType>> GetProjectedEntityListAsync<T, TProjectedType>(
            Expression<Func<T, bool>> condition,
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            List<TProjectedType> projectedEntites = await query.Select(selectExpression).ToListAsync();

            return projectedEntites;
        }

        public async Task<List<TProjectedType>> GetProjectedListAsync<T, TProjectedType>(
            Expression<Func<T, bool>> condition,
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            List<TProjectedType> projectedEntites = await query.Select(selectExpression).ToListAsync();

            return projectedEntites;
        }

        [Obsolete]
        public async Task<List<TProjectedType>> GetProjectedEntityListAsync<T, TProjectedType>(
            Specification<T> specification,
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            return await query.Select(selectExpression).ToListAsync();
        }

        public async Task<List<TProjectedType>> GetProjectedListAsync<T, TProjectedType>(
            Specification<T> specification,
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            return await query.Select(selectExpression).ToListAsync();
        }

        [Obsolete]
        public async Task<T> GetEntityByIdAsync<T>(object id, bool asNoTracking = false)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            T trackedEntity = await GetByIdAsync<T>(id, asNoTracking);
            return trackedEntity;
        }

        public async Task<T> GetByIdAsync<T>(object id)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            T trackedEntity = await GetByIdAsync<T>(id, false);
            return trackedEntity;
        }

        public async Task<T> GetByIdAsync<T>(object id, bool asNoTracking)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            T trackedEntity = await GetByIdAsync<T>(id, null, asNoTracking);
            return trackedEntity;
        }

        public async Task<T> GetByIdAsync<T>(object id, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            T trackedEntity = await GetByIdAsync<T>(id, includes, false);
            return trackedEntity;
        }

        public async Task<T> GetByIdAsync<T>(object id, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes, bool asNoTracking = false)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(T));

            string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
            Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primayKeyValue = null;

            try
            {
                primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }

            ParameterExpression pe = Expression.Parameter(typeof(T), "entity");
            MemberExpression me = Expression.Property(pe, primaryKeyName);
            ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<T, bool>> expressionTree = Expression.Lambda<Func<T, bool>>(body, new[] { pe });

            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            T trackedEntity = await query.FirstOrDefaultAsync(expressionTree);
            return trackedEntity;
        }

        [Obsolete]
        public async Task<TProjectedType> GetProjectedEntityByIdAsync<T, TProjectedType>(
            object id,
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(T));

            string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
            Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primayKeyValue = null;

            try
            {
                primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }

            ParameterExpression pe = Expression.Parameter(typeof(T), "entity");
            MemberExpression me = Expression.Property(pe, primaryKeyName);
            ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<T, bool>> expressionTree = Expression.Lambda<Func<T, bool>>(body, new[] { pe });

            IQueryable<T> query = _dbContext.Set<T>();

            return await query.Where(expressionTree).Select(selectExpression).FirstOrDefaultAsync();
        }

        public async Task<TProjectedType> GetProjectedByIdAsync<T, TProjectedType>(
            object id,
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(T));

            string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
            Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primayKeyValue = null;

            try
            {
                primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }

            ParameterExpression pe = Expression.Parameter(typeof(T), "entity");
            MemberExpression me = Expression.Property(pe, primaryKeyName);
            ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<T, bool>> expressionTree = Expression.Lambda<Func<T, bool>>(body, new[] { pe });

            IQueryable<T> query = _dbContext.Set<T>();

            return await query.Where(expressionTree).Select(selectExpression).FirstOrDefaultAsync();
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> condition, bool asNoTracking = false)
           where T : class
        {
            return await GetAsync(condition, asNoTracking);
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> condition)
           where T : class
        {
            return await GetAsync(condition, null, false);
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> condition, bool asNoTracking)
           where T : class
        {
            return await GetAsync(condition, null, asNoTracking);
        }

        public async Task<T> GetAsync<T>(
            Expression<Func<T, bool>> condition,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
           where T : class
        {
            return await GetAsync(condition, includes, false);
        }

        public async Task<T> GetAsync<T>(
            Expression<Func<T, bool>> condition,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            bool asNoTracking)
           where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        [Obsolete]
        public async Task<T> GetEntityAsync<T>(Specification<T> specification, bool asNoTracking = false)
            where T : class
        {
            return await GetAsync(specification, asNoTracking);
        }

        public async Task<T> GetAsync<T>(Specification<T> specification)
            where T : class
        {
            return await GetAsync(specification, false);
        }

        public async Task<T> GetAsync<T>(Specification<T> specification, bool asNoTracking)
            where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        [Obsolete]
        public async Task<TProjectedType> GetProjectedEntityAsync<T, TProjectedType>(
            Expression<Func<T, bool>> condition,
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return await query.Select(selectExpression).FirstOrDefaultAsync();
        }

        public async Task<TProjectedType> GetProjectedAsync<T, TProjectedType>(
            Expression<Func<T, bool>> condition,
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return await query.Select(selectExpression).FirstOrDefaultAsync();
        }

        [Obsolete]
        public async Task<TProjectedType> GetProjectedEntityAsync<T, TProjectedType>(
            Specification<T> specification,
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            return await query.Select(selectExpression).FirstOrDefaultAsync();
        }

        public async Task<TProjectedType> GetProjectedAsync<T, TProjectedType>(
            Specification<T> specification,
            Expression<Func<T, TProjectedType>> selectExpression)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            return await query.Select(selectExpression).FirstOrDefaultAsync();
        }

        [Obsolete]
        public async Task<bool> IsEntityExistsAsync<T>(Expression<Func<T, bool>> condition)
           where T : class
        {
            return await ExistsAsync<T>(condition);
        }

        public async Task<bool> ExistsAsync<T>()
           where T : class
        {
            return await ExistsAsync<T>(null);
        }

        public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> condition)
           where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (condition == null)
            {
                return await query.AnyAsync();
            }

            bool isExists = await query.AnyAsync(condition);
            return isExists;
        }

        [Obsolete]
        public async Task<object[]> InsertEntityAsync<T>(T entity)
           where T : class
        {
            return await InsertAsync<T>(entity);
        }

        public async Task<object[]> InsertAsync<T>(T entity, CancellationToken cancellationToken = default)
           where T : class
        {
            EntityEntry<T> entityEntry = await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            object[] primaryKeyValue = entityEntry.Metadata.FindPrimaryKey().Properties.
                Select(p => entityEntry.Property(p.Name).CurrentValue).ToArray();

            return primaryKeyValue;
        }

        public async Task InsertAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
           where T : class
        {
            await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        [Obsolete]
        public async Task InsertEntitiesAsync<T>(IEnumerable<T> entities)
            where T : class
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        [Obsolete]
        public void UpdateEntity<T>(T entity)
            where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            UpdateAsync<T>(entity).GetAwaiter().GetResult();
        }

        public async Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            EntityEntry<T> trackedEntity = _dbContext.ChangeTracker.Entries<T>().FirstOrDefault(x => x.Entity == entity);

            if (trackedEntity == null)
            {
                IEntityType entityType = _dbContext.Model.FindEntityType(typeof(T));

                if (entityType == null)
                {
                    throw new InvalidOperationException($"{typeof(T).Name} is not part of EF Core DbContext model");
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

                _dbContext.Set<T>().Update(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task UpdateAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            _dbContext.Set<T>().UpdateRange(entities);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        [Obsolete]
        public void UpdateEntities<T>(IEnumerable<T> entities)
            where T : class
        {
            _dbContext.Set<T>().UpdateRange(entities);
            _dbContext.SaveChanges();
        }

        [Obsolete]
        public void DeleteEntity<T>(T entity)
            where T : class
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public async Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        [Obsolete]
        public void DeleteEntities<T>(IEnumerable<T> entities)
            where T : class
        {
            _dbContext.Set<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public async Task DeleteAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync<T>()
            where T : class
        {
            int count = await _dbContext.Set<T>().CountAsync();
            return count;
        }

        public async Task<int> GetCountAsync<T>(params Expression<Func<T, bool>>[] conditions)
            where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (conditions == null)
            {
                return await query.CountAsync();
            }

            foreach (Expression<Func<T, bool>> expression in conditions)
            {
                query = query.Where(expression);
            }

            return await query.CountAsync();
        }

        public async Task<long> GetLongCountAsync<T>()
            where T : class
        {
            long count = await _dbContext.Set<T>().LongCountAsync();
            return count;
        }

        public async Task<long> GetLongCountAsync<T>(params Expression<Func<T, bool>>[] conditions)
            where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (conditions == null)
            {
                return await query.LongCountAsync();
            }

            foreach (Expression<Func<T, bool>> expression in conditions)
            {
                query = query.Where(expression);
            }

            return await query.LongCountAsync();
        }

        // DbConext level members
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlRaw(sql, parameters);
        }

        public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        public void ResetContextState()
        {
#if NETCOREAPP3_1
            _dbContext.ChangeTracker.Entries().Where(e => e.Entity != null).ToList()
                            .ForEach(e => e.State = EntityState.Detached);
#else
            _dbContext.ChangeTracker.Clear();
#endif
        }
    }
}