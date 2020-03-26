using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.Repository
{
    internal class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TEntity>> GetEntityListAsync(Specification<TEntity> specification)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            return await query.ToListAsync();
        }

        public async Task<List<TProjectedType>> GetEntityListAsync<TProjectedType>(Specification<TEntity> specification,
            Expression<Func<TEntity, TProjectedType>> selectExpression) where TProjectedType : class
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

        public async Task<TEntity> GetEntityByIdAsync(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            TEntity entity = await _dbContext.Set<TEntity>().FindAsync(id);
            return entity;
        }

        public async Task<TProjectedType> GetEntityByIdAsync<TProjectedType>(object id, Expression<Func<TEntity, TProjectedType>> selectExpression)
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

            var primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
            var primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            Type primaryKeyValueType = id.GetType();

            if (primaryKeyType != primaryKeyValueType)
            {
                throw new ArgumentException($"You can not assign a value of type {primaryKeyValueType} to a property of type {primaryKeyType}");
            }

            ParameterExpression pe = Expression.Parameter(typeof(TEntity), "entity");
            MemberExpression me = Expression.Property(pe, primaryKeyName);
            ConstantExpression constant = Expression.Constant(id, primaryKeyValueType);
            BinaryExpression body = Expression.Equal(me, constant);
            var expressionTree = Expression.Lambda<Func<TEntity, bool>>(body, new[] { pe });

            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            return await query.Where(expressionTree).Select(selectExpression).FirstOrDefaultAsync();

        }

        public async Task<TEntity> GetEntityAsync(Specification<TEntity> specification)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<TProjectedType> GetEntityAsync<TProjectedType>(Specification<TEntity> specification,
            Expression<Func<TEntity, TProjectedType>> selectExpression)
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
            return condition != null && await _dbContext.Set<TEntity>().AnyAsync(condition);
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
