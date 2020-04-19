using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetEntityListAsync(Specification<TEntity> specification);
        Task<List<TProjectedType>> GetEntityListAsync<TProjectedType>(Specification<TEntity> specification,
            Expression<Func<TEntity, TProjectedType>> selectExpression) where TProjectedType : class;
        Task<TEntity> GetEntityByIdAsync(object id);
        Task<TProjectedType> GetEntityByIdAsync<TProjectedType>(object id, Expression<Func<TEntity, TProjectedType>> selectExpression);
        Task<TEntity> GetEntityAsync(Specification<TEntity> specification);
        Task<TProjectedType> GetEntityAsync<TProjectedType>(Specification<TEntity> specification, Expression<Func<TEntity, TProjectedType>> selectExpression);
        Task<bool> IsEntityExistsAsync(Expression<Func<TEntity, bool>> condition);
        Task InsertEntityAsync(TEntity entity);
        Task InsertEntitiesAsync(IEnumerable<TEntity> entities);
        void UpdateEntity(TEntity entity);
        void UpdateEntities(IEnumerable<TEntity> entities);
        void DeleteEntity(TEntity entity);
        void DeleteEntities(IEnumerable<TEntity> entities);
        Task<int> GetCountAsync(params Expression<Func<TEntity, bool>>[] conditions);
        Task<long> GetLongCountAsync(params Expression<Func<TEntity, bool>>[] conditions);
    }
}
