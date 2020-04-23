// <copyright file="Repository.cs" company="TanvirArjel">
// Copyright (c) TanvirArjel. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using TanvirArjel.EFCore.GenericRepository.Services;

namespace TanvirArjel.EFCore.GenericRepository.Implementations
{
    internal class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            Entities = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Entities { get; }

        public async Task<List<TEntity>> GetEntityListAsync(bool asNoTracking = false)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            List<TEntity> entities = await query.ToListAsync();

            return entities;
        }

        public async Task<List<TEntity>> GetEntityListAsync(Expression<Func<TEntity, bool>> condition, bool asNoTracking = false)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            List<TEntity> entities = await query.ToListAsync();

            return entities;
        }

        public async Task<List<TEntity>> GetEntityListAsync(Specification<TEntity> specification, bool asNoTracking = false)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

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

        public async Task<List<TProjectedType>> GetProjectedEntityListAsync<TProjectedType>(Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TProjectedType : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            List<TProjectedType> entities = await _dbContext.Set<TEntity>().Select(selectExpression).ToListAsync();

            return entities;
        }

        public async Task<List<TProjectedType>> GetProjectedEntityListAsync<TProjectedType>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TProjectedType : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            List<TProjectedType> projectedEntites = await query.Select(selectExpression).ToListAsync();

            return projectedEntites;
        }

        public async Task<List<TProjectedType>> GetProjectedEntityListAsync<TProjectedType>(
            Specification<TEntity> specification,
            Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TProjectedType : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            return await query.Select(selectExpression).ToListAsync();
        }

        public async Task<TEntity> GetEntityByIdAsync(object id, bool asNoTracking = false)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));

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

            ParameterExpression pe = Expression.Parameter(typeof(TEntity), "entity");
            MemberExpression me = Expression.Property(pe, primaryKeyName);
            ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<TEntity, bool>> expressionTree = Expression.Lambda<Func<TEntity, bool>>(body, new[] { pe });

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (asNoTracking)
            {
                TEntity noTrackedEntity = await query.AsNoTracking().FirstOrDefaultAsync(expressionTree);
                return noTrackedEntity;
            }

            TEntity trackedEntity = await query.FirstOrDefaultAsync(expressionTree);
            return trackedEntity;
        }

        public async Task<TProjectedType> GetProjectedEntityByIdAsync<TProjectedType>(
            object id,
            Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TProjectedType : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IEntityType entityType = _dbContext.Model.FindEntityType(typeof(TEntity));

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

            ParameterExpression pe = Expression.Parameter(typeof(TEntity), "entity");
            MemberExpression me = Expression.Property(pe, primaryKeyName);
            ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<TEntity, bool>> expressionTree = Expression.Lambda<Func<TEntity, bool>>(body, new[] { pe });

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            return await query.Where(expressionTree).Select(selectExpression).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> condition, bool asNoTracking = false)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetEntityAsync(Specification<TEntity> specification, bool asNoTracking = false)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

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

        public async Task<TProjectedType> GetProjectedEntityAsync<TProjectedType>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TProjectedType : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return await query.Select(selectExpression).FirstOrDefaultAsync();
        }

        public async Task<TProjectedType> GetProjectedEntityAsync<TProjectedType>(
            Specification<TEntity> specification,
            Expression<Func<TEntity, TProjectedType>> selectExpression)
            where TProjectedType : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            return await query.Select(selectExpression).FirstOrDefaultAsync();
        }

        public async Task<bool> IsEntityExistsAsync(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                return false;
            }

            bool isExists = await _dbContext.Set<TEntity>().AnyAsync(condition);
            return isExists;
        }

        public async Task InsertEntityAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task InsertEntitiesAsync(IEnumerable<TEntity> entities)
        {
            await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        }

        public void UpdateEntity(TEntity entity)
        {
            EntityEntry<TEntity> trackedEntity = _dbContext.ChangeTracker.Entries<TEntity>().FirstOrDefault(x => x.Entity == entity);

            if (trackedEntity != null)
            {
                _dbContext.Entry(entity).CurrentValues.SetValues(entity);
            }
            else
            {
                _dbContext.Set<TEntity>().Update(entity);
            }
        }

        public void UpdateEntities(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().UpdateRange(entities);
        }

        public void DeleteEntity(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void DeleteEntities(IEnumerable<TEntity> entities)
        {
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<int> GetCountAsync(params Expression<Func<TEntity, bool>>[] conditions)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (conditions == null)
            {
                return await query.CountAsync();
            }

            foreach (Expression<Func<TEntity, bool>> expression in conditions)
            {
                query = query.Where(expression);
            }

            return await query.CountAsync();
        }

        public async Task<long> GetLongCountAsync(params Expression<Func<TEntity, bool>>[] conditions)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (conditions == null)
            {
                return await query.LongCountAsync();
            }

            foreach (Expression<Func<TEntity, bool>> expression in conditions)
            {
                query = query.Where(expression);
            }

            return await query.LongCountAsync();
        }
    }
}
